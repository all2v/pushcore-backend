using Astrum.SharedLib.Domain.Entities;

namespace Astrum.CodeRev.Domain.Aggregates;

public class ReviewerDraft : AggregateRootBase<Guid>
{
    public Guid InterviewSolutionId { get; set; }
    public Draft.Draft? Draft { get; set; }
}