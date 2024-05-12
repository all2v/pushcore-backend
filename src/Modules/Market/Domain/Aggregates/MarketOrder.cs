using Astrum.SharedLib.Domain.Entities;

namespace Astrum.Market.Aggregates;

public class MarketOrder : AggregateRootBase<Guid>
{
    public Guid UserId { get; set; }
    public string? Comment { get; set; }
    public string? SellerResponse { get; set; }
    public List<OrderProduct> OrderProducts { get; set; }
    public int Status { get; set; }
}