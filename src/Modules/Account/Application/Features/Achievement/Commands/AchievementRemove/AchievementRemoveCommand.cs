using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.Account.Features.Achievement;
using Astrum.SharedLib.Common.CQS.Implementations;

namespace Astrum.Account.Application.Features.Achievement.Commands.AchievementRemove
{
    public class AchievementRemoveCommand : CommandResult<UserAchievementResponse>
    {
        public string Username { get; set; }
        public Guid AchievementId { get; set; }
    }
}
