using Astrum.Account.Application.Features.Achievement.Commands.AchievementRemove;
using Astrum.Account.Features.Achievement;
using Astrum.Account.Features.Achievement.Commands.AchievementAssign;

namespace Astrum.Account.Services;

public interface IUserAchievementService
{
    public Task<UserAchievementResponse> AssignAchievementAsync(AchievementAssignCommand command);
    public Task<UserAchievementResponse> RemoveAchievementAsync(AchievementRemoveCommand command);
}