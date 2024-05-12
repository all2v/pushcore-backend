using Ardalis.Specification;
using Astrum.Account.Aggregates;
using Astrum.SharedLib.Persistence.Repositories;

namespace Astrum.Account.Repositories;

public class SocialNetworksRepository : EFRepository<SocialNetworks, Guid, AccountDbContext>, ISocialNetworksRepository
{
    public SocialNetworksRepository(AccountDbContext context, ISpecificationEvaluator? specificationEvaluator = null) :
        base(context, specificationEvaluator)
    {
    }
}