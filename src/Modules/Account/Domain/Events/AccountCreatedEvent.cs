using Astrum.SharedLib.Domain.Events;
using Astrum.SharedLib.Domain.Interfaces;

namespace Astrum.Account.Events;

public class AccountCreatedEvent : DomainEventBase<Guid>
{
    public AccountCreatedEvent(Guid userId, Aggregates.Account account) :
        base(Guid.NewGuid())
    {
        UserId = userId;
        Account = account;
    }

    public AccountCreatedEvent(Guid aggregateId, int aggregateVersion, Guid userId,
        Aggregates.Account account)
        : base(aggregateId, aggregateVersion)
    {
        UserId = userId;
        Account = account;
    }

    public Guid UserId { get; set; }
    public Aggregates.Account Account { get; set; }

    public override IDomainEvent<Guid> WithAggregate(Guid aggregateId, int aggregateVersion)
    {
        return new AccountCreatedEvent(aggregateId, aggregateVersion, UserId, Account);
    }
}