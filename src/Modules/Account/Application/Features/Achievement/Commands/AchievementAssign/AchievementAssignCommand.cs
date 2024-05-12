using Astrum.SharedLib.Common.CQS.Implementations;
using Microsoft.AspNetCore.Mvc;

namespace Astrum.Account.Features.Achievement.Commands.AchievementAssign;

public class AchievementAssignCommand : CommandResult<UserAchievementResponse>
{
    public string Username { get; set; }
    public Guid AchievementId { get; set; }
}