using Ardalis.Specification;
using Astrum.CodeRev.Domain.Aggregates;

namespace Astrum.CodeRev.Domain.Specifications.Interviews;

public class GetAsynchronousSubmittedInterviewSolutions : Specification<InterviewSolution>
{
    public GetAsynchronousSubmittedInterviewSolutions()
    {
        Query
            .Where(interviewSolution =>
                !interviewSolution.IsSynchronous || interviewSolution.IsSubmittedByCandidate)
            .Include(interview => interview.Interview)
            .Include(interview => interview.TaskSolutions)
            .AsSplitQuery();
    }
}