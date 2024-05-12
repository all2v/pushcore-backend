using Astrum.SharedLib.Domain.Entities;

namespace Astrum.CodeRev.Domain.Aggregates;

public class InterviewTask : AggregateRootBase<Guid>
{
    public Guid TestTask { get; set; }
    public Guid InterviewId { get; set; }
}