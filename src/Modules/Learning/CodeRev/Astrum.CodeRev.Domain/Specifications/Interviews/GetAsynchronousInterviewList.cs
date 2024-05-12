using Ardalis.Specification;
using Astrum.CodeRev.Domain.Aggregates;

namespace Astrum.CodeRev.Domain.Specifications.Interviews;

public class GetAsynchronousInterviewList : Specification<Interview>
{
    public GetAsynchronousInterviewList()
    {
        Query.Include(interview => interview.InterviewSolutions
                .Where(interviewSolution =>
                    !interviewSolution.IsSynchronous || interviewSolution.IsSubmittedByCandidate))
            .ThenInclude(interview => interview.TaskSolutions);
    }
}