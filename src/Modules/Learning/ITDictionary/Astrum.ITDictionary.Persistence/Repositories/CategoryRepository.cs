using Ardalis.Specification;
using Astrum.ITDictionary.Aggregates;
using Astrum.SharedLib.Persistence.Repositories;

namespace Astrum.ITDictionary.Repositories;

public class CategoryRepository: EFRepository<Category, Guid, ITDictionaryDbContext>, ICategoryRepository
{
    public CategoryRepository(ITDictionaryDbContext context, ISpecificationEvaluator? specificationEvaluator = null) : base(context, specificationEvaluator)
    {
    }
}