using Astrum.SharedLib.Domain.Entities;

namespace Astrum.Recruting.Domain.Aggregates.Board;

public class CandidateLink : AggregateRootBase<Guid>
{
    public Guid HrmUserId { get; set; }
    public Stage Stage { get; set; }
    public Guid StageId { get; set; }
    public List<StageEvent> StageEvents { get; set; }
}