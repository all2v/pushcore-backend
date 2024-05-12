using Ardalis.Specification;
using Astrum.ITDictionary.Aggregates;

namespace Astrum.ITDictionary.Repositories;

public class UserTermRepository: BaseRepository<UserTerm, ITDictionaryDbContext>, IUserTermRepository
{
    public UserTermRepository(ITDictionaryDbContext context, ISpecificationEvaluator? specificationEvaluator = null) : base(context, specificationEvaluator)
    {
    }
}