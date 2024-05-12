using Astrum.SharedLib.Common.CQS.Implementations;
using Astrum.Storage.ViewModels;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Astrum.Account.Features.Achievement.Commands.AchievementCreate;

public class AchievementCreateCommand : CommandResult<AchievementResponse>
{
    public FileForm Icon { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}
