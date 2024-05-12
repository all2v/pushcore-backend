using Astrum.SharedLib.Domain.Entities;

namespace Astrum.Recruting.Domain.Aggregates.Board;

public class CandidateLinkHistory : AggregateRootBase<Guid>
{
    public Guid ChangedBy { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public StageEvent StageEvent { get; set; }
}