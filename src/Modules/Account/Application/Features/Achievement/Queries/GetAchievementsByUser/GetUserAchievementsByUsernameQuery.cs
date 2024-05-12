using Astrum.SharedLib.Common.CQS.Implementations;

namespace Astrum.Account.Features.Achievement.Queries.GetAchievementsByUser;

public class GetUserAchievementsByUsernameQuery : QueryResult<List<AchievementResponse>>
{
    public GetUserAchievementsByUsernameQuery(string username)
    {
        Username = username;
    }

    public string Username { get; }
}