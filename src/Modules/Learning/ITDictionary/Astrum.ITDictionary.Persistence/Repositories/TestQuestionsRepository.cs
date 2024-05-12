using Ardalis.Specification;
using Astrum.ITDictionary.Aggregates;
using Astrum.SharedLib.Persistence.Repositories;

namespace Astrum.ITDictionary.Repositories;

public class TestQuestionsRepository : EFRepository<TestQuestion, Guid, ITDictionaryDbContext>, ITestQuestionRepository
{
    public TestQuestionsRepository(ITDictionaryDbContext context,
        ISpecificationEvaluator? specificationEvaluator = null) : base(context, specificationEvaluator)
    {
    }
}