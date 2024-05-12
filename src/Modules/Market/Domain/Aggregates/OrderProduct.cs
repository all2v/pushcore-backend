using Astrum.SharedLib.Domain.Entities;

namespace Astrum.Market.Aggregates;

public class OrderProduct : AggregateRootBase<Guid>
{
    public Guid ProductId { get; set; }
    public MarketProduct Product { get; set; }
    public Guid OrderId { get; set; }
    public int Status { get; set; }
    public int Amount { get; set; }
}