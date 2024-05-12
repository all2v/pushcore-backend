using Astrum.SharedLib.Common.CQS.Implementations;
using Astrum.Storage.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Astrum.Account.Features.Achievement.Commands.AchievementUpdate;

public class AchievementUpdateCommand : CommandResult<AchievementResponse>
{
    [FromRoute]
    public Guid Id { get; set; }

    public FileForm Icon { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}