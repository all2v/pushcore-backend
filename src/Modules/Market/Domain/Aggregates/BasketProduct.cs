using Astrum.SharedLib.Domain.Entities;

namespace Astrum.Market.Aggregates;

public class BasketProduct : AggregateRootBase<Guid>
{
    public int Amount { get; set; }

    public Guid ProductId { get; set; }
    public MarketProduct Product { get; set; }

    public Guid BasketId { get; set; }
}