using Ardalis.Specification;
using Astrum.ITDictionary.Aggregates;

namespace Astrum.ITDictionary.Specifications;

public class GetAnswerOptionSpec: Specification<QuestionAnswerOption>
{
    public GetAnswerOptionSpec()
    {
        Query
            .Include(e => e.TermSource);
    }
}