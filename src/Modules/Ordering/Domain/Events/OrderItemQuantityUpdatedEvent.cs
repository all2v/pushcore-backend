using Astrum.SharedLib.Domain.Events;
using Astrum.SharedLib.Domain.Interfaces;

namespace Astrum.Ordering.Events;

public class OrderItemQuantityUpdatedEvent : DomainEventBase<Guid>
{
    /// <summary>
    ///     Needed for serialization
    /// </summary>
    internal OrderItemQuantityUpdatedEvent()
    {
    }

    public OrderItemQuantityUpdatedEvent(Guid orderItemId, int quantity)
    {
        OrderItemId = orderItemId;
        Quantity = quantity;
    }

    public OrderItemQuantityUpdatedEvent(Guid aggregateId, int aggregateVersion, Guid orderItemId, int quantity)
        : base(aggregateId, aggregateVersion)
    {
        OrderItemId = orderItemId;
        Quantity = quantity;
    }

    public Guid OrderItemId { get; }
    public int Quantity { get; }

    public override IDomainEvent<Guid> WithAggregate(Guid aggregateId, int aggregateVersion)
    {
        return new OrderItemQuantityUpdatedEvent(aggregateId, aggregateVersion, OrderItemId, Quantity);
    }
}