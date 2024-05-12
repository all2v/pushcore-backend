using Ardalis.Specification;

namespace Astrum.Market.Specifications.MarketOrder;

public class GetMarketOrderByIdSpec : GetMarketOrdersSpec
{
    public GetMarketOrderByIdSpec(Guid id)
    {
        Query
            .Where(x => x.Id == id);
    }
}