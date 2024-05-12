using Astrum.SharedLib.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Astrum.SharedLib.Persistence.Helpers;

/// <summary>
/// </summary>
public static class DomainEventsHelper
{
    /// <summary>
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="changeTracker"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="TId"></typeparam>
    public static async Task DispatchDomainEventsAsync<TId>(this IMediator mediator, ChangeTracker changeTracker,
        CancellationToken cancellationToken = default)
    {
        var domainEntities = changeTracker
            .Entries<IAggregateRoot<TId>>()
            .Where(x => x.Entity.GetUncommittedEvents().Any())
            .ToList();

        var domainEvents = domainEntities
            .SelectMany(x => x.Entity.GetUncommittedEvents())
            .ToList();

        domainEntities.ForEach(entry => entry.Entity.ClearUncommittedEvents());

        var tasks = domainEvents
            .Select(async domainEvent => { await mediator.Publish(domainEvent, cancellationToken); }).ToArray();

        await Task.WhenAll(tasks);
    }
}