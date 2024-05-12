using Astrum.SharedLib.Domain.Entities;

namespace Astrum.Recruting.Domain.Aggregates.Candidate;

public class JobExperience : AggregateRootBase<Guid>
{
    public DateTimeOffset StartAt { get; set; }
    public DateTimeOffset EndAt { get; set; }
    public string CompanyName { get; set; }
    public string Position { get; set; }
    public CandidateResume CandidateResume { get; set; }
    public string CandidateResumeId { get; set; }
}