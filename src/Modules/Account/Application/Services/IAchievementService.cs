using Astrum.Account.Features.Achievement;
using Astrum.Account.Features.Achievement.Commands.AchievementCreate;
using Astrum.Account.Features.Achievement.Commands.AchievementUpdate;
using Astrum.SharedLib.Common.Results;

namespace Astrum.Account.Services;

public interface IAchievementService
{
    public Task<Result<AchievementResponse>> CreateAsync(AchievementCreateCommand command);
    public Task<Result<AchievementResponse>> GetAsync(Guid achievementId);
    public Task<Result<List<AchievementResponse>>> GetUserAchievementsByUsernameAsync(string username);
    public Task<Result<AchievementResponse>> UpdateAsync(AchievementUpdateCommand command);
    public Task<Result<AchievementResponse>> DeleteAsync(Guid achievementId);
}