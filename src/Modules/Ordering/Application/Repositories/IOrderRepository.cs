using Astrum.Ordering.Aggregates;
using Astrum.SharedLib.Application.Contracts.Persistence.Repositories;

namespace Astrum.Ordering.Repositories;

public interface IOrderRepository : IEntityRepository<Order, Guid>
{
    Task SaveToEventStoreAsync(Order order);
}