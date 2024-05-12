using System.Collections.Generic;
using System.Linq;
using Astrum.Application.Entities;
using Astrum.Application.Enums;
using Astrum.SharedLib.Application.Contracts.Infrastructure.EventStore;
using Astrum.SharedLib.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Astrum.Application.EventStore;

/// <inheritdoc cref="IRetroactiveEventsService" />
/// <summary>
///     Service to handle applying retroactive events to the event stream
/// </summary>
internal class RetroactiveEventsService : IRetroactiveEventsService
{
    private const string STREAM_NAME = "Main";
    private readonly DbSet<BranchPoint> _branchPoints;
    private readonly ILogger<RetroactiveEventsService> _logger;

    public RetroactiveEventsService(ILogger<RetroactiveEventsService> logger, EventStoreDbContext eventStoreDbContext)
    {
        _logger = logger;
        _branchPoints = eventStoreDbContext.Set<BranchPoint>();
    }

    #region IRetroactiveEventsService Members

    public IReadOnlyCollection<IDomainEvent<TAggregateId>> ApplyRetroactiveEventsToStream<T, TAggregateId>(
        IReadOnlyCollection<IDomainEvent<TAggregateId>> eventStream) where T : class, IAggregateRoot<TAggregateId>
    {
        var allBranchPointsForEvents = _branchPoints.Include(bp => bp.RetroactiveEvents).Where(bp =>
            eventStream.Select(es => es.EventId).Contains(bp.EventId) && bp.Name == STREAM_NAME).ToList();
        if (!allBranchPointsForEvents.Any())
            return eventStream;

        var newEventStream = new List<IDomainEvent<TAggregateId>>();

        foreach (var @event in eventStream.OrderBy(e => e.AggregateVersion))
        {
            var branchPointForEvent = allBranchPointsForEvents.SingleOrDefault(bp => bp.EventId == @event.EventId);
            if (branchPointForEvent != null)
            {
                //insert events for branch point
                var currentBranchPointEvents = branchPointForEvent.RetroactiveEvents.OrderBy(re => re.Sequence);
                newEventStream.AddRange(
                    GetBranchPointEvents(@event, currentBranchPointEvents, branchPointForEvent.Type));
            }
            else
                newEventStream.Add(@event);
        }

        return newEventStream;
    }

    #endregion

    private List<IDomainEvent<TAggregateId>> GetBranchPointEvents<TAggregateId>(IDomainEvent<TAggregateId> currentEvent,
        IOrderedEnumerable<RetroactiveEvent> retroactiveEvents, BranchPointTypeEnum branchPointType)
    {
        var events = new List<IDomainEvent<TAggregateId>>();
        switch (branchPointType)
        {
            case BranchPointTypeEnum.OutOfOrder:
            {
                events.Add(currentEvent);
                events.AddRange(ConstructDomainEvents<TAggregateId>(retroactiveEvents));
            }
                break;
            case BranchPointTypeEnum.Incorrect:
                events.AddRange(ConstructDomainEvents<TAggregateId>(retroactiveEvents));
                break;
            case BranchPointTypeEnum.Rejected:
            default: break;
        }

        return events;
    }

    private IEnumerable<IDomainEvent<TAggregateId>> ConstructDomainEvents<TAggregateId>(
        IOrderedEnumerable<RetroactiveEvent> retroactiveEvents)
    {
        return retroactiveEvents.Select(re =>
            DomainEventHelper.ConstructDomainEvent<TAggregateId>(re.Data, re.AssemblyTypeName));
    }
}