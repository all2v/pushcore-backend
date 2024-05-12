using Ardalis.Specification;
using Astrum.Market.Aggregates;
using Astrum.SharedLib.Persistence.Repositories;

namespace Astrum.Market.Repositories;

public class MarketBasketRepository : EFRepository<MarketBasket, Guid, MarketDbContext>,
    IMarketBasketRepository
{
    public MarketBasketRepository(MarketDbContext context, ISpecificationEvaluator? specificationEvaluator = null) :
        base(context, specificationEvaluator)
    {
    }
}