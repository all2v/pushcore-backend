using Ardalis.Specification;

namespace Astrum.Market.Specifications.MarketBasket;

public class GetMarketBasketByIdSpec : GetMarketBasketsSpec
{
    public GetMarketBasketByIdSpec(Guid id)
    {
        Query
            .Where(x => x.Id == id);
    }
}

public class GetMarketBasketByOwnerIdSpec : GetMarketBasketsSpec
{
    public GetMarketBasketByOwnerIdSpec(Guid id)
    {
        Query
            .Where(x => x.Owner == id);
    }
}