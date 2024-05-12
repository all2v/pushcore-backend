using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.Account.Features.Achievement;
using Astrum.SharedLib.Common.CQS.Implementations;

namespace Astrum.Account.Application.Features.Achievement.Queries.GetAchievementsList
{
    public class GetAchievementsListQuery : QueryResult<List<AchievementResponse>>
    {
    }
}
