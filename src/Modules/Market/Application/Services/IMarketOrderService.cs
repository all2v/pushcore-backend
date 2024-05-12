using Astrum.Market.ViewModels;

namespace Astrum.Market.Services;

public interface IMarketOrderService
{
    Task<SharedLib.Common.Results.Result<List<MarketOrderFormResponse>>> GetAllOrders();
    Task<SharedLib.Common.Results.Result<MarketOrderFormResponse>> GetOrder(Guid id);
    Task<SharedLib.Common.Results.Result<MarketOrderFormResponse>> Add(MarketOrderFormRequest order);
    Task<SharedLib.Common.Results.Result<MarketOrderFormResponse>> Remove(Guid id);
    Task<SharedLib.Common.Results.Result<MarketOrderFormResponse>> Update(Guid id, MarketOrderFormRequest order);
}