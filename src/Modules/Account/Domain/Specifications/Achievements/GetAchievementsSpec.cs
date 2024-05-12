using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Specification;
using Astrum.Account.Aggregates;

namespace Astrum.Account.Specifications.Achievements
{
    public class GetAchievementsSpec : Specification<Achievement>
    {
        public GetAchievementsSpec() 
        {
            Query
                .OrderByDescending(achievement => achievement.DateCreated);
        }
    }

    public class GetAchievementByIdSpec : GetAchievementsSpec
    {
        public GetAchievementByIdSpec(Guid achievementId) 
        { 
            Query
                .Where(achievement => achievement.Id == achievementId);
        }
    }
}
