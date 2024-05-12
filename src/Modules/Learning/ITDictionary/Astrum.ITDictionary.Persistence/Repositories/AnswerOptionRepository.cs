using Ardalis.Specification;
using Astrum.ITDictionary.Aggregates;
using Astrum.SharedLib.Persistence.Repositories;

namespace Astrum.ITDictionary.Repositories;

public class AnswerOptionRepository : EFRepository<QuestionAnswerOption, Guid, ITDictionaryDbContext>,
    IAnswerOptionRepository
{
    public AnswerOptionRepository(ITDictionaryDbContext context, ISpecificationEvaluator? specificationEvaluator = null)
        : base(context, specificationEvaluator)
    {
    }
}