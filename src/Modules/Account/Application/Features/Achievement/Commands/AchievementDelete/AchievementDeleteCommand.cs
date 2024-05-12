using Astrum.SharedLib.Common.CQS.Implementations;
using Microsoft.AspNetCore.Mvc;

namespace Astrum.Account.Features.Achievement.Commands.AchievementDelete;

public class AchievementDeleteCommand : CommandResult<AchievementResponse>
{
    public AchievementDeleteCommand([FromRoute] Guid Id)
    {
        this.Id = Id;
    }

    public Guid Id { get; init; }

    public void Deconstruct([FromRoute] out Guid Id)
    {
        Id = this.Id;
    }
}