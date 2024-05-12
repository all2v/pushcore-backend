using Ardalis.Specification;
using Astrum.CodeRev.Domain.Aggregates;

namespace Astrum.CodeRev.Domain.Specifications.Interviews;

public class GetInterviewSolutionById : Specification<InterviewSolution>
{
    public GetInterviewSolutionById(Guid interviewSolutionId)
    {
        Query
            .Where(solution => solution.Id == interviewSolutionId)
            .Include(solution => solution.Interview);
    }
}