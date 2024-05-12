namespace Astrum.SharedLib.Domain.Interfaces;

public interface IDomainEventHandler<in T> where T : IDomainEvent
{
    void Apply(T @event);
}