using Ardalis.Specification;
using Astrum.Account.Aggregates;
using Astrum.SharedLib.Persistence.Repositories;

namespace Astrum.Account.Repositories;

public class UserProfileRepository : EFRepository<UserProfile, Guid, AccountDbContext>, IUserProfileRepository
{
    public UserProfileRepository(AccountDbContext context, ISpecificationEvaluator? specificationEvaluator = null) :
        base(context, specificationEvaluator)
    {
    }
}