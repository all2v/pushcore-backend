using Astrum.Market.ViewModels;
using HotChocolate;
using HotChocolate.Types;

namespace Astrum.Market.GraphQL;

public class SubscriptionMarket
{
    [Subscribe]
    public IEnumerable<MarketProductFormRequest> ProductsChanged([EventMessage] IEnumerable<MarketProductFormRequest> products)
    {
        return products;
    }

    [Subscribe]
    public MarketProductFormRequest ProductChanged([EventMessage] MarketProductFormRequest product)
    {
        return product;
    }

    [Subscribe]
    public IEnumerable<MarketOrderFormResponse> OrdersChanged([EventMessage] IEnumerable<MarketOrderFormResponse> orders)
    {
        return orders;
    }
}