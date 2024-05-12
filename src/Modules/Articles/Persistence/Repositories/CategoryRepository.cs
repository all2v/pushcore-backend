using Ardalis.Specification;
using Astrum.Articles.Aggregates;
using Astrum.SharedLib.Persistence.Repositories;

namespace Astrum.Articles.Repositories;

internal class CategoryRepository : EFRepository<Category, Guid, ArticlesDbContext>,
    ICategoryRepository
{
    public CategoryRepository(ArticlesDbContext context, ISpecificationEvaluator? specificationEvaluator = null) : base(
        context, specificationEvaluator)
    {
    }
}