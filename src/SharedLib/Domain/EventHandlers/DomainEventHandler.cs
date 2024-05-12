using Astrum.SharedLib.Domain.Interfaces;
using MediatR;

namespace Astrum.SharedLib.Domain.EventHandlers;

public abstract class DomainEventHandler<T> : INotificationHandler<T>  where T : IDomainEvent
{
    public abstract Task Handle(T @event, CancellationToken cancellationToken);
}