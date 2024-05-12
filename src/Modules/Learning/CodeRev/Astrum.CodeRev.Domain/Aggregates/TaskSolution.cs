using Astrum.CodeRev.Domain.Aggregates.Enums;
using Astrum.SharedLib.Domain.Entities;

namespace Astrum.CodeRev.Domain.Aggregates;

public class TaskSolution : AggregateRootBase<Guid>
{
    public Guid TaskId { get; set; }
    public TestTask TestTask { get; set; }
    public InterviewSolution InterviewSolution { get; set; }
    public Guid InterviewSolutionId { get; set; }
    public bool IsDone { get; set; }
    public Grade Grade { get; set; }
    public int RunAttemptsLeft { get; set; }

}