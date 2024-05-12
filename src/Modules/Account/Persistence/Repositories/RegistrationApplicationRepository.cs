using Ardalis.Specification;
using Astrum.Account.Aggregates;
using Astrum.SharedLib.Persistence.Repositories;

namespace Astrum.Account.Repositories;

public class RegistrationApplicationRepository : EFRepository<RegistrationApplication, Guid, AccountDbContext>,
    IRegistrationApplicationRepository
{
    public RegistrationApplicationRepository(AccountDbContext context,
        ISpecificationEvaluator? specificationEvaluator = null) : base(context, specificationEvaluator)
    {
    }
}