using Astrum.SharedLib.Domain.Entities;

namespace Astrum.Account.Aggregates;

public class UserAchievement : AggregateRootBase<Guid>
{
    private readonly List<Achievement> _achievements = new();

    private UserAchievement()
    {
    }

    public UserAchievement(Guid userId)
    {
        UserId = userId;
    }

    public Guid UserId { get; }

    public IEnumerable<Achievement> Achievements => _achievements;

    public void AddAchievement(Achievement achievement)
    {
        _achievements.Add(achievement);
    }
}