using Ardalis.Specification;

namespace Astrum.Market.Specifications.MarketOrder;

public class GetMarketOrderWithProductSpec : Specification<Aggregates.MarketOrder>
{
    public GetMarketOrderWithProductSpec()
    {
        Query.Include(x => x.OrderProducts).ThenInclude(x => x.Product).AsNoTracking();
    }
}