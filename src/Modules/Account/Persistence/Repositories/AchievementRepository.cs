using Ardalis.Specification;
using Astrum.Account.Aggregates;
using Astrum.SharedLib.Persistence.Repositories;

namespace Astrum.Account.Repositories;

public class AchievementRepository : EFRepository<Achievement, Guid, AccountDbContext>,
    IAchievementRepository
{
    public AchievementRepository(AccountDbContext context, ISpecificationEvaluator? specificationEvaluator = null) :
        base(context, specificationEvaluator)
    {
    }
}