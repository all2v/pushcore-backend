using Astrum.SharedLib.Domain.Entities;

namespace Astrum.Market.Aggregates;

public class MarketProduct : AggregateRootBase<Guid>
{
    private MarketProduct() { }

    public MarketProduct(Guid id)
    {
        Id = id;
    }

    public string Name { get; set; }
    public string Summary { get; set; }
    public string? Description { get; set; }
    public double Price { get; set; }
    public int Remain { get; set; }
    public bool IsAvailable { get; set; }
    public bool IsInfinite { get; set; } = false;
    public List<BasketProduct> Baskets { get; set; }
    public List<OrderProduct> Orders { get; set; }
    public Guid? CoverImageId { get; set; }
}