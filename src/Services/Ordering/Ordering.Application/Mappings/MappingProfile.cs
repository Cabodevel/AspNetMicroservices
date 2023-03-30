using AutoMapper;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;
using Ordering.Application.Features.Orders.Commands.UpdateOrder;
using Ordering.Application.Features.Orders.Queries.GetOrdersList;
using Ordering.Domain.Entities;

namespace Ordering.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Order, OrderVm>().ReverseMap();
            CreateMap<Order, CheckoutOrderCommand>();
            CreateMap<CheckoutOrderCommand, Order>()
                .ForMember(x => x.LastModifiedDate, y => DateTime.UtcNow.ToString("s"))
                .ForMember(x => x.LastModifiedBy, y => DateTime.UtcNow.ToString("s"));
            CreateMap<Order, UpdateOrderCommand>().ReverseMap();
        }
    }
}
