using Ardalis.Specification;
using Astrum.Account.Aggregates;
using Astrum.SharedLib.Persistence.Repositories;

namespace Astrum.Account.Repositories;

public class UserAchievementRepository : EFRepository<UserAchievement, Guid, AccountDbContext>,
    IUserAchievementRepository
{
    public UserAchievementRepository(AccountDbContext context, ISpecificationEvaluator? specificationEvaluator = null) :
        base(context, specificationEvaluator)
    {
    }
}