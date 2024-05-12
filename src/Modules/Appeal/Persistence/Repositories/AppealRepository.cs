using Ardalis.Specification;
using Astrum.SharedLib.Persistence.Repositories;

namespace Astrum.Appeal.Repositories;

/// <summary>
///     Implementation of <see cref="IAppealRepository" />
///     store.
/// </summary>
public class AppealRepository : EFRepository<Aggregates.Appeal, Guid, AppealDbContext>,
    IAppealRepository
{
    public AppealRepository(AppealDbContext context, ISpecificationEvaluator? specificationEvaluator = null) : base(
        context, specificationEvaluator)
    {
    }
}