using Astrum.Account.Services;
using Astrum.SharedLib.Common.Results;

namespace Astrum.Account.Features.Achievement.Commands.AchievementDelete;

public class
    AchievementDeleteCommandHandler : BaseCommandAchievementHandler<AchievementDeleteCommand, AchievementResponse>
{
    public AchievementDeleteCommandHandler(IAchievementService achievementService) : base(achievementService)
    {
    }

    public override async Task<Result<AchievementResponse>> Handle(AchievementDeleteCommand command,
        CancellationToken cancellationToken = default)
    {
        var response = await AchievementService.DeleteAsync(command.Id);
        return response;
    }
}