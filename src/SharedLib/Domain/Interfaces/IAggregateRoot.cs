namespace Astrum.SharedLib.Domain.Interfaces;

/// <summary>
///     Interface for aggregate root
/// </summary>
public interface IAggregateRoot<TId> : IBaseEntity<TId>, IAggregateRoot
{
    /// <summary>
    ///     The aggregate root current version
    /// </summary>
    int Version { get; }

    void ApplyEvent(IDomainEvent<TId> @event, int version);
    IEnumerable<IDomainEvent> GetUncommittedEvents();
    void ClearUncommittedEvents();
    public void AddEvent(IDomainEvent @event);
    public void RemoveEvent(IDomainEvent @event);
}

public interface IAggregateRoot
{
}