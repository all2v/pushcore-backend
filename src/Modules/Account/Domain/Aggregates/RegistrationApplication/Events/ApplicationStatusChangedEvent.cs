using Astrum.Account.Enums;
using Astrum.SharedLib.Domain.Events;
using Astrum.SharedLib.Domain.Interfaces;

namespace Astrum.Account.Aggregates;

public class ApplicationStatusChangedEvent : DomainEventBase<Guid>
{
    public ApplicationStatus Status { get; }

    public ApplicationStatusChangedEvent(ApplicationStatus status)
    {
        Status = status;
    }

    public ApplicationStatusChangedEvent(Guid aggregateId, int aggregateVersion, ApplicationStatus status)
        : base(aggregateId, aggregateVersion)
    {
        Status = status;
    }

    public override IDomainEvent<Guid> WithAggregate(Guid aggregateId, int aggregateVersion)
    {
        return new ApplicationStatusChangedEvent(aggregateId, aggregateVersion, Status);
    }
}