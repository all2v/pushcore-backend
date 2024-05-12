using Ardalis.Specification;

namespace Astrum.Market.Specifications.MarketOrder;

public class GetMarketOrderByIdAsNoTrackingSpec : GetMarketOrderByIdSpec
{
    public GetMarketOrderByIdAsNoTrackingSpec(Guid id) : base(id)
    {
        Query
            .AsNoTracking();
    }
}