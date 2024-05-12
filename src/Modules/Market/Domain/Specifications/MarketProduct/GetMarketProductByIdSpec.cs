using Ardalis.Specification;

namespace Astrum.Market.Specifications.MarketProduct;

public class GetMarketProductByIdSpec : GetMarketProductsSpec
{
    public GetMarketProductByIdSpec(Guid id)
    {
        Query
            .Where(x => x.Id == id);
    }
}