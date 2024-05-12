using Ardalis.Specification;
using Astrum.ITDictionary.Aggregates;
using Astrum.SharedLib.Persistence.Repositories;

namespace Astrum.ITDictionary.Repositories;

public class TermRepository: EFRepository<Term, Guid, ITDictionaryDbContext>, ITermRepository
{
    public TermRepository(ITDictionaryDbContext context, ISpecificationEvaluator? specificationEvaluator = null) : base(context, specificationEvaluator)
    {
    }
}