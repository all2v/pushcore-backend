using Ardalis.Specification;
using Astrum.Appeal.Aggregates;
using Astrum.SharedLib.Persistence.Repositories;

namespace Astrum.Appeal.Repositories;

public class AppealCategoryRepository : EFRepository<AppealCategory, Guid, AppealDbContext>,
    IAppealCategoryRepository
{
    public AppealCategoryRepository(AppealDbContext context, ISpecificationEvaluator? specificationEvaluator = null) :
        base(
            context, specificationEvaluator)
    {
    }
}
