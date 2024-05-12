using Ardalis.Specification;
using Astrum.CodeRev.Domain.Aggregates;

namespace Astrum.CodeRev.Domain.Specifications.Interviews;

public class GetInterviewByIdWithTasks : Specification<Interview>
{
    public GetInterviewByIdWithTasks(Guid id)
    {
        Query
            .Where(interview => interview.Id == id)
            .Include(interview => interview.Tasks);
    }
}