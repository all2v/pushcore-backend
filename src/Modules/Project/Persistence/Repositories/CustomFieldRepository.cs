using Ardalis.Specification;
using Astrum.Projects.Aggregates;
using Astrum.SharedLib.Persistence.Repositories;

namespace Astrum.Projects.Repositories;

public class CustomFieldRepository : EFRepository<CustomField, Guid, ProjectDbContext>,
    ICustomFieldRepository
{
    public CustomFieldRepository(ProjectDbContext context, ISpecificationEvaluator? specificationEvaluator = null) :
        base(context, specificationEvaluator)
    {
    }
}