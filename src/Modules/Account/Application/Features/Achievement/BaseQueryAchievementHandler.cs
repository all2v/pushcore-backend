using Astrum.Account.Services;
using Astrum.SharedLib.Common.CQS.Implementations;
using Astrum.SharedLib.Common.Results;

namespace Astrum.Account.Features.Achievement;

public abstract class BaseQueryAchievementHandler<TQuery, TResponse> : QueryHandler<TQuery, TResponse>
    where TQuery : Query<TResponse> where TResponse : class, IResult
{
    protected readonly IAchievementService AchievementService;

    public BaseQueryAchievementHandler(IAchievementService achievementService)
    {
        AchievementService = achievementService;
    }
}