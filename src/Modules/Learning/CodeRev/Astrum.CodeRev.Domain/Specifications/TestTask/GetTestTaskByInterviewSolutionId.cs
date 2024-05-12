using Ardalis.Specification;

namespace Astrum.CodeRev.Domain.Specifications.TestTask;

public class GetTestTaskByInterviewSolutionId : Specification<CodeRev.Domain.Aggregates.TaskSolution>
{
    public GetTestTaskByInterviewSolutionId(Guid interviewSolutionId)
    {
        Query
            .Where(solution => solution.InterviewSolution.Id == interviewSolutionId)
            .Include(solution => solution.TestTask);
    }
}