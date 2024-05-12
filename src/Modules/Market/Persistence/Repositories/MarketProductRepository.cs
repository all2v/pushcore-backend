using Ardalis.Specification;
using Astrum.Market.Aggregates;
using Astrum.SharedLib.Persistence.Repositories;

namespace Astrum.Market.Repositories;

public class MarketProductRepository : EFRepository<MarketProduct, Guid, MarketDbContext>,
    IMarketProductRepository
{
    public MarketProductRepository(MarketDbContext context, ISpecificationEvaluator? specificationEvaluator = null) :
        base(context, specificationEvaluator)
    {
    }
}