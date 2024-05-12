using Ardalis.Specification;

namespace Astrum.CodeRev.Domain.Specifications.TaskSolution;

public class GetTaskSolutionByIdWithInterviewSolution : Specification<CodeRev.Domain.Aggregates.TaskSolution>
{
    public GetTaskSolutionByIdWithInterviewSolution(Guid taskSolutionId)
    {
        Query
            .Where(solution => solution.Id == taskSolutionId)
            .Include(solution => solution.InterviewSolution);
    }
}