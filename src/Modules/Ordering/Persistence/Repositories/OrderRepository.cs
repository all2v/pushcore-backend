using Ardalis.Specification;
using Astrum.Ordering.Aggregates;
using Astrum.SharedLib.Application.Contracts.Persistence.Repositories;
using Astrum.SharedLib.Persistence.Repositories;

namespace Astrum.Ordering.Repositories;

internal class OrderRepository : EFRepository<Order, Guid, OrderingDbContext>, IOrderRepository
{
    private readonly IESRepository<Order, Guid> _eventStoreRepository;

    public OrderRepository(OrderingDbContext context,
        ISpecificationEvaluator? specificationEvaluator = null) : base(context, specificationEvaluator)
    {
    }

    #region IOrderRepository Members


    #endregion

    public Task SaveToEventStoreAsync(Order order)
    {
        throw new NotImplementedException();
    }
}