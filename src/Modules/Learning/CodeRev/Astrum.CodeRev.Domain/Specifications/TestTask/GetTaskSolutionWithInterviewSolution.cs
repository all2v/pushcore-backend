using Ardalis.Specification;

namespace Astrum.CodeRev.Domain.Specifications.TestTask;

public class GetTaskSolutionWithInterviewSolution : Specification<CodeRev.Domain.Aggregates.TaskSolution>
{
    public GetTaskSolutionWithInterviewSolution(Guid taskSolutionId)
    {
        Query
            .Where(solution => solution.Id == taskSolutionId)
            .Include(solution => solution.InterviewSolution);
    }
}