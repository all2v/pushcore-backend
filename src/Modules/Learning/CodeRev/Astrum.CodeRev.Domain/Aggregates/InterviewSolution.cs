using Astrum.CodeRev.Domain.Aggregates.Enums;
using Astrum.SharedLib.Domain.Entities;

namespace Astrum.CodeRev.Domain.Aggregates;

public class InterviewSolution : AggregateRootBase<Guid>
{
    public Guid UserId { get; set; }

    public string Username { get; set; }
    public string FirstName { get; set; }
    public string Surname { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }

    // public Guid InterviewId { get; set; }
    //public Guid TaskSolutionId { get; set; }
    public long StartTimeMs { get; set; }
    public long EndTimeMs { get; set; }
    public long TimeToCheckMs { get; set; } // fixed time until which solution must be checked
    public string ReviewerComment { get; set; }
    public Grade AverageGrade { get; set; }
    public InterviewResult InterviewResult { get; set; }
    public bool IsSubmittedByCandidate { get; set; }
    public Guid InvitedBy { get; set; }
    public bool IsSynchronous { get; set; }
    public ReviewerDraft ReviewerDraft { get; set; }
    public Guid ReviewerDraftId { get; set; }

    public Interview Interview { get; set; }
    public IEnumerable<TaskSolution> TaskSolutions { get; set; }
}