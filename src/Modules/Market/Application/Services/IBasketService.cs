using Astrum.Market.ViewModels;

namespace Astrum.Market.Services;

public interface IBasketService
{
    Task<SharedLib.Common.Results.Result<BasketForm>> GetBasket(Guid owner);
    Task<SharedLib.Common.Results.Result<BasketForm>> CreateBasket(Guid userId);
    Task<SharedLib.Common.Results.Result<BasketForm>> AddProductAsync(Guid basketId, Guid productId, int amount = 1);
    Task<SharedLib.Common.Results.Result<BasketForm>> RemoveProduct(Guid basketId, Guid productId, int amount = 1);
}