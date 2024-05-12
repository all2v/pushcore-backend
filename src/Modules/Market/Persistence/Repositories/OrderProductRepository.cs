using Ardalis.Specification;
using Astrum.Market.Aggregates;
using Astrum.SharedLib.Persistence.Repositories;

namespace Astrum.Market.Repositories;

/// <summary>
///     Implementation of <see cref="IMarketOrderRepository" /> which allows persistence on both EventStore and relational
///     store.
/// </summary>
public class OrderProductRepository : EFRepository<OrderProduct, Guid, MarketDbContext>,
    IOrderProductRepository
{
    public OrderProductRepository(MarketDbContext context, ISpecificationEvaluator? specificationEvaluator = null) :
        base(context, specificationEvaluator)
    {
    }
}