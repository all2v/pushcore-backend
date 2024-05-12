using Astrum.Account.Services;
using Astrum.SharedLib.Common.CQS.Implementations;
using Astrum.SharedLib.Common.CQS.Interfaces;
using Astrum.SharedLib.Common.Results;

namespace Astrum.Account.Features.Achievement;

public abstract class BaseCommandAchievementHandler<TCommand, TResponse> : CommandResultHandler<TCommand, TResponse>
    where TCommand : class, ICommand<Result<TResponse>>
    where TResponse : class
{
    protected readonly IAchievementService AchievementService;

    public BaseCommandAchievementHandler(IAchievementService achievementService)
    {
        AchievementService = achievementService;
    }
}