using Ardalis.Specification;
using Astrum.ITDictionary.Aggregates;
using Astrum.SharedLib.Persistence.Repositories;

namespace Astrum.ITDictionary.Repositories;

public class PracticeRepository : EFRepository<Practice, Guid, ITDictionaryDbContext>, IPracticeRepository
{
    public PracticeRepository(ITDictionaryDbContext context, ISpecificationEvaluator? specificationEvaluator = null) :
        base(context, specificationEvaluator)
    {
    }
}