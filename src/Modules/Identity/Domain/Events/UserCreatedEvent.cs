using Astrum.SharedLib.Domain.Events;
using Astrum.SharedLib.Domain.Interfaces;

namespace Astrum.Identity.Events;

/// <summary>
///     User Created domain event
/// </summary>
public class UserCreatedEvent : DomainEventBase<Guid>
{
    /// <summary>
    ///     Needed for serialization
    /// </summary>
    // internal UserCreatedEvent()
    // {
    // }

    public UserCreatedEvent(string username, string email, string name)
    {
        Username = username;
        Email = email;
        Name = name;
    }

    public UserCreatedEvent(Guid aggregateId, int aggregateVersion, string username, string email, string name)
        : base(aggregateId, aggregateVersion)
    {
        Username = username;
        Email = email;
        Name = name;
    }

    public string Username { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }

    public override IDomainEvent<Guid> WithAggregate(Guid aggregateId, int aggregateVersion)
    {
        return new UserCreatedEvent(aggregateId, aggregateVersion, Username, Email, Name);
    }
}