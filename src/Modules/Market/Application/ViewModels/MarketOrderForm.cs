using Astrum.Account.Features.Profile;

namespace Astrum.Market.ViewModels;

public class MarketOrderFormRequest
{
    public Guid UserId { get; set; }
    public string? Comment { get; set; }
    public string? SellerResponse { get; set; }
    public List<OrderProductFormRequest>? OrderProducts { get; set; }
    public OrderStatus? Status { get; set; } = OrderStatus.Ordered;
}

public class MarketOrderFormResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }

    public string? Comment { get; set; }
    public string? SellerResponse { get; set; }
    public UserProfileResponse? User { get; set; }
    public List<OrderProductFormResponse>? OrderProducts { get; set; }
    public OrderStatus? Status { get; set; } = OrderStatus.Ordered;
}