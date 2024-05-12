using System.Reflection;
using Astrum.SharedLib.Domain.Events;
using Astrum.SharedLib.Domain.Interfaces;

namespace Astrum.SharedLib.Domain.Entities;

// TODO Should inherits BaseEntity (AuditableEntity сделан для упрощения)
public abstract class AggregateRootBase<TId> : AuditableEntity<TId>, IAggregateRoot<TId>
{
    public const int NewAggregateVersion = -1;

    private readonly ICollection<IDomainEvent> _domainEvents = new LinkedList<IDomainEvent>();

    #region IAggregateRoot<TId> Members

    public int Version { get; private set; } = NewAggregateVersion;

    void IAggregateRoot<TId>.ApplyEvent(IDomainEvent<TId> @event, int version)
    {
        if (!_domainEvents.Any(x => Equals(x.EventId, @event.EventId)))
            try
            {
                InvokeHandler(@event);
                Version = version;
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException;
            }

        Version = version;
    }

    void IAggregateRoot<TId>.ClearUncommittedEvents()
    {
        _domainEvents.Clear();
    }

    public void AddEvent(IDomainEvent @event)
    {
        _domainEvents.Add(@event);
    }

    public void RemoveEvent(IDomainEvent @event)
    {
        _domainEvents.Remove(@event);
    }

    IEnumerable<IDomainEvent> IAggregateRoot<TId>.GetUncommittedEvents()
    {
        return _domainEvents.AsEnumerable();
    }

    #endregion


    protected void RaiseEvent<TEvent>(TEvent @event)
        where TEvent : DomainEventBase<TId>
    {
        var version = Version + 1;
        var eventWithAggregate = @event.WithAggregate(
            Equals(Id, default(TId)) ? @event.AggregateId : Id,
            version);

        ((IAggregateRoot<TId>)this).ApplyEvent(eventWithAggregate, version);
        _domainEvents.Add(eventWithAggregate);
    }

    private void InvokeHandler(IDomainEvent<TId> @event)
    {
        var handlerMethod = GetEventHandlerMethodInfo(@event);
        handlerMethod.Invoke(this, new object[] {@event});
    }

    private MethodInfo GetEventHandlerMethodInfo(IDomainEvent<TId> @event)
    {
        var handlerType = GetType()
            .GetInterfaces()
            .Where(t => t.IsGenericType)
            .Single(i =>
                i.GetGenericTypeDefinition() == typeof(IDomainEventHandler<>) &&
                i.GetGenericArguments()[0] == @event.GetType());
        var eventHandler = handlerType.GetTypeInfo().GetDeclaredMethod(nameof(IDomainEventHandler<IDomainEvent>.Apply));
        if (eventHandler is null)
            throw new Exception("Domain event handler must exist");
        return eventHandler;
    }
}