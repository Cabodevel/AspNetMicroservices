using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Ordering.API.Extensions
{
    public static class WebApplicationExtensions
    {
        public static async Task MigrateDatabase<TContext>(this IHost host,
                                            Action<TContext, IServiceProvider> seeder,
                                            int? retry = 0) where TContext : DbContext
        {
            int retryForAvailability = retry.Value;

            using var scope = host.Services.CreateAsyncScope();
            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILogger<TContext>>();
            var context = services.GetService<TContext>();
           
            try
            {
                
                logger.LogInformation("Migrating database associated with context {DbContextName}", typeof(TContext).Name);

                await InvokeSeeder(seeder, context, services);

                logger.LogInformation("Migrated database associated with context {DbContextName}", typeof(TContext).Name);
            }
            catch (SqlException ex)
            {
                logger.LogError(ex, "An error occurred while migrating the database used on context {DbContextName}", typeof(TContext).Name);

                if (retryForAvailability < 50)
                {
                    retryForAvailability++;
                    Thread.Sleep(2000);
                    await MigrateDatabase<TContext>(host, seeder, retryForAvailability);
                }
            }
        }

        private static async Task InvokeSeeder<TContext>(Action<TContext, IServiceProvider> seeder,
                                                    TContext context,
                                                    IServiceProvider services)
                                                    where TContext : DbContext
        {
            await context.Database.MigrateAsync();
            //seeder(context, services);
        }
    }
}
