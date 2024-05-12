using Astrum.SharedLib.Domain.Events;
using Astrum.SharedLib.Domain.Interfaces;

namespace Astrum.Ordering.Events;

public class OrderItemAddedEvent : DomainEventBase<Guid>
{
    /// <summary>
    ///     Needed for serialization
    /// </summary>
    internal OrderItemAddedEvent()
    {
    }

    public OrderItemAddedEvent(string productName, decimal productPrice, int quantity)
    {
        ProductName = productName;
        Quantity = quantity;
        ProductPrice = productPrice;
    }

    public OrderItemAddedEvent(Guid aggregateId, int aggregateVersion, string productName, decimal productPrice,
        int quantity)
        : base(aggregateId, aggregateVersion)
    {
        ProductName = productName;
        Quantity = quantity;
        ProductPrice = productPrice;
    }

    public string ProductName { get; }
    public int Quantity { get; }
    public decimal ProductPrice { get; }

    public override IDomainEvent<Guid> WithAggregate(Guid aggregateId, int aggregateVersion)
    {
        return new OrderItemAddedEvent(aggregateId, aggregateVersion, ProductName, ProductPrice, Quantity);
    }
}