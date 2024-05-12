using Astrum.SharedLib.Domain.Interfaces;

namespace Astrum.SharedLib.Domain.Events;

public abstract class DomainEventBase<TAggregateId> : IDomainEvent<TAggregateId>,
    IEquatable<DomainEventBase<TAggregateId>>
{
    protected DomainEventBase()
    {
        EventId = Guid.NewGuid();
    }

    protected DomainEventBase(TAggregateId? aggregateId) : this()
    {
        AggregateId = aggregateId;
    }

    protected DomainEventBase(TAggregateId aggregateId, int aggregateVersion) : this(aggregateId)
    {
        AggregateVersion = aggregateVersion;
    }

    #region IDomainEvent<TAggregateId> Members

    public Guid EventId { get; }

    public TAggregateId AggregateId { get; }

    public int AggregateVersion { get; }

    #endregion

    #region IEquatable<DomainEventBase<TAggregateId>> Members

    public bool Equals(DomainEventBase<TAggregateId>? other)
    {
        return other != null &&
               EventId.Equals(other.EventId);
    }

    #endregion

    public override bool Equals(object? obj)
    {
        return this.Equals(obj as DomainEventBase<TAggregateId>);
    }

    public override int GetHashCode()
    {
        return 290933282 + EqualityComparer<Guid>.Default.GetHashCode(EventId);
    }

    public abstract IDomainEvent<TAggregateId> WithAggregate(TAggregateId aggregateId, int aggregateVersion);
}