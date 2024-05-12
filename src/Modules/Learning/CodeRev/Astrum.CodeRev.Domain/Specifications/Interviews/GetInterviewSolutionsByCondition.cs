using System.Linq.Expressions;
using Ardalis.Specification;
using Astrum.CodeRev.Domain.Aggregates;

namespace Astrum.CodeRev.Domain.Specifications.Interviews;

public class GetInterviewSolutionsByCondition : Specification<InterviewSolution>
{
    public GetInterviewSolutionsByCondition(Expression<Func<InterviewSolution, bool>> predicate, int offset, int limit)
    {
        Query.Where(predicate)
            .Include(solution => solution.Interview)
            .ThenInclude(interview => interview.Tasks)
            .ThenInclude(task => task.TaskSolutions)
            .Skip(offset)
            .Take(limit);
    }
}