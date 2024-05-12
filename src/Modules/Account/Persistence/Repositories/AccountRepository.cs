using Ardalis.Specification;
using Astrum.SharedLib.Persistence.Repositories;

namespace Astrum.Account.Repositories;

/// <summary>
///     Implementation of <see cref="IAccountRepository" /> which allows persistence on both EventStore and relational
///     store.
/// </summary>
public class AccountRepository : EFRepository<Astrum.Account.Aggregates.Account, Guid, AccountDbContext>,
    IAccountRepository
{
    public AccountRepository(AccountDbContext context, ISpecificationEvaluator? specificationEvaluator = null) : base(
        context, specificationEvaluator)
    {
    }
}