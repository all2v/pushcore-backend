using Astrum.Market.Aggregates;
using Astrum.Market.ViewModels;
using Astrum.SharedLib.Common.Results;
using Astrum.Storage.Application.ViewModels;
using Astrum.Storage.ViewModels;

namespace Astrum.Market.Services;

public interface IProductService
{
    Task<SharedLib.Common.Results.Result<List<MarketProductFormResponse>>> GetAllProducts();
    Task<SharedLib.Common.Results.Result<MarketProductFormResponse>> GetProduct(Guid productId);
    Task<SharedLib.Common.Results.Result<MarketProductFormResponse>> Create(MarketProductFormRequest marketProduct, FileForm? image = null);
    Task<SharedLib.Common.Results.Result<MarketProductFormResponse>> Delete(Guid id);
    Task<SharedLib.Common.Results.Result<MarketProductFormResponse>> Update(Guid productId, MarketProductFormRequest marketProduct);
    Task<SharedLib.Common.Results.Result<MarketProductFormResponse>> Replenish(Guid productId, int amount);
    Task<SharedLib.Common.Results.Result<MarketProductFormResponse>> Reduce(Guid productId, int amount);
    Task SetProductImageUrls(List<MarketProductFormResponse> products);
}