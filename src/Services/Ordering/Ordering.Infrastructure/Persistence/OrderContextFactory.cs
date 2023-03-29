using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;


namespace Ordering.Infrastructure.Persistence
{
    public class OrderContextFactory : IDesignTimeDbContextFactory<OrderContext>
    {
       

        public OrderContext CreateDbContext(string[] args)
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var startupProjectPath = Path.Combine(currentDirectory, "../Ordering.API");
            var basePathConfiguration = Directory.Exists(startupProjectPath) ? startupProjectPath : currentDirectory;

            var configurationBuilder = new ConfigurationBuilder();
            var configuration = configurationBuilder
                .SetBasePath(basePathConfiguration)
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<OrderContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("OrderingConnectionString"));

            return new OrderContext(optionsBuilder.Options);
        }
    }
}
