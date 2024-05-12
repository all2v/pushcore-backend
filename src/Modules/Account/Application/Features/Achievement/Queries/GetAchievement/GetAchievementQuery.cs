using Astrum.SharedLib.Common.CQS.Implementations;

namespace Astrum.Account.Features.Achievement.Queries.GetAchievement;

public class GetAchievementQuery : QueryResult<AchievementResponse>
{
    public GetAchievementQuery(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; }
}