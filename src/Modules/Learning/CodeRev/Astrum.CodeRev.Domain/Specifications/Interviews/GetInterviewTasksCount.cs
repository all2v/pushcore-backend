using Ardalis.Specification;
using Astrum.CodeRev.Domain.Aggregates;

namespace Astrum.CodeRev.Domain.Specifications.Interviews;

public class GetInterviewTasksCount : Specification<Interview>
{
    public GetInterviewTasksCount(Guid id)
    {
        Query
            .Where(interview  => interview.Id==id)
            ;
    }
}