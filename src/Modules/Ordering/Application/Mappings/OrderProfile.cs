using Astrum.Ordering.Aggregates;
using Astrum.Ordering.ReadModels;
using Astrum.SharedLib.Application.ReadModels;
using Astrum.SharedLib.Domain.ValueObjects;
using AutoMapper;

namespace Astrum.Ordering.Mappings;

internal class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<Order, OrderReadModel>();
        CreateMap<OrderItem, OrderItemReadModel>();
        CreateMap<Address, AddressReadModel>();
    }
}
