using Ardalis.Specification;

namespace Astrum.Market.Specifications.MarketOrder;

public class GetMarketOrdersSpec : Specification<Aggregates.MarketOrder>
{
    public GetMarketOrdersSpec()
    {
        Query
            .Include(x => x.OrderProducts);
    }
}