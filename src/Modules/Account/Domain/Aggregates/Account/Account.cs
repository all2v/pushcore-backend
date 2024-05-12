using Ardalis.GuardClauses;
using Astrum.SharedLib.Domain.Entities;

namespace Astrum.Account.Aggregates;

public class Account : AggregateRootBase<Guid>
{
    public Account(string userId)
    {
        Guard.Against.NullOrWhiteSpace(userId);
        // TODO raise event
        UserId = userId;
    }

    public string UserId { get; protected set; }
    public string? AvatarUrl { get; set; }
    public string PhoneNumber { get; set; } // TODO to validated value object
}