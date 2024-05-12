using Astrum.SharedLib.Domain.Entities;

namespace Astrum.Ordering.Aggregates;

public class OrderItem : AggregateRootBase<Guid>
{
    public OrderItem(string productName, decimal productPrice, int quantity)
    {
        ProductName = productName;
        ProductPrice = productPrice;
        Quantity = quantity;
    }

    public string ProductName { get; }
    public decimal ProductPrice { get; }
    public int Quantity { get; set; }

    public Guid Id { get; private set; }


    internal void UpdateQuantity(int quantity)
    {
        Quantity = quantity;
    }
}