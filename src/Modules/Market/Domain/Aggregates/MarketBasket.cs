using Astrum.SharedLib.Domain.Entities;

namespace Astrum.Market.Aggregates;

public class MarketBasket : AggregateRootBase<Guid>
{
    private readonly List<BasketProduct> _products = new();
    private MarketBasket() { }

    public MarketBasket(Guid owner)
    {
        Owner = owner;
    }

    public Guid Owner { get; set; }

    public IEnumerable<BasketProduct> Products => _products;

    public void AddProducts(params BasketProduct[] products)
    {
        // TODO validate
        _products.AddRange(products);
    }

    public void RemoveProducts(params BasketProduct[] products)
    {
        // TODO validate
        foreach (var product in products)
            _products.Remove(product);
    }
}