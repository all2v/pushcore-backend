using Astrum.Market.Services;
using Astrum.Market.ViewModels;
using HotChocolate;
using HotChocolate.Types;

namespace Astrum.Market.GraphQL;

public class QueryMarket
{
    public async Task<BasketForm> GetBasket([Service] IBasketService basketService, Guid owner)
    {
        return await basketService.GetBasket(owner);
    }

    [UsePaging(MaxPageSize = 20, IncludeTotalCount = true)]
    [UseSorting]
    [UseFiltering]
    public async Task<List<MarketOrderFormResponse>> GetOrderList([Service] IMarketOrderService marketOrderService)
    {
        return await marketOrderService.GetAllOrders();
    }

    public async Task<MarketOrderFormResponse> GetOrder([Service] IMarketOrderService marketOrderService, Guid id)
    {
        return await marketOrderService.GetOrder(id);
    }

    [UsePaging(MaxPageSize = 20, IncludeTotalCount = true)]
    [UseSorting]
    [UseFiltering]
    public async Task<List<MarketProductFormResponse>> GetProductList([Service] IProductService productService)
    {
        return await productService.GetAllProducts();
    }

    public async Task<MarketProductFormResponse> GetProduct([Service] IProductService productService, Guid id)
    {
        return await productService.GetProduct(id);
    }
}