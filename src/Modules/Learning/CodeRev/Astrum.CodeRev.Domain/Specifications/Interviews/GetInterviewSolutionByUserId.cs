using Ardalis.Specification;
using Astrum.CodeRev.Domain.Aggregates;

namespace Astrum.CodeRev.Domain.Specifications.Interviews;

public class GetInterviewSolutionByUserId : Specification<InterviewSolution>
{
    public GetInterviewSolutionByUserId(Guid userId)
    {
        Query.Where(solution => solution.UserId == userId)
            .Include(solution => solution.Interview);
    }
}