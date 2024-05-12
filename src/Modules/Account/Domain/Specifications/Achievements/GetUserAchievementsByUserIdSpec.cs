using Ardalis.Specification;
using Astrum.Account.Aggregates;

namespace Astrum.Account.Specifications.Achievements;

public class GetUserAchievementsSpec : Specification<UserAchievement>
{
    public GetUserAchievementsSpec()
    {
        //Query
        //    .Include(x => x.Achievements);
    }
}

public class GetUserAchievementsByUserIdSpec : GetUserAchievementsSpec
{
    public GetUserAchievementsByUserIdSpec(Guid userId)
    {
        //Query
        //    .Where(x => x.UserId == userId);
    }
}