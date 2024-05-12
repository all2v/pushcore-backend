using Astrum.SharedLib.Domain.Events;
using Astrum.SharedLib.Domain.Interfaces;

namespace Astrum.Ordering.Events;

public class OrderCreatedEvent : DomainEventBase<Guid>
{
    /// <summary>
    ///     Needed for serialization
    /// </summary>
    internal OrderCreatedEvent()
    {
    }

    public OrderCreatedEvent(string trackingNumber) : base(Guid.NewGuid())
    {
        TrackingNumber = trackingNumber;
    }

    public OrderCreatedEvent(Guid aggregateId, int aggregateVersion, string trackingNumber)
        : base(aggregateId, aggregateVersion)
    {
        TrackingNumber = trackingNumber;
    }

    public string TrackingNumber { get; set; }

    public override IDomainEvent<Guid> WithAggregate(Guid aggregateId, int aggregateVersion)
    {
        return new OrderCreatedEvent(aggregateId, aggregateVersion, TrackingNumber);
    }
}