using Astrum.SharedLib.Common.CQS.Implementations;

namespace Astrum.Account.Features.Achievement.Queries.GetAchievementsByUser;

public class GetAchievementsByUserQuery : QueryResult<List<AchievementResponse>>
{
    public GetAchievementsByUserQuery(Guid userId)
    {
        UserId = userId;
    }

    public Guid UserId { get; }
}