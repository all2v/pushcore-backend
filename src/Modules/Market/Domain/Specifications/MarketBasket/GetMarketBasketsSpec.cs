using Ardalis.Specification;

namespace Astrum.Market.Specifications.MarketBasket;

public class GetMarketBasketsSpec : Specification<Aggregates.MarketBasket>
{
    public GetMarketBasketsSpec()
    {
        Query.Include(x => x.Products)
            .ThenInclude(x => x.Product);
    }
}