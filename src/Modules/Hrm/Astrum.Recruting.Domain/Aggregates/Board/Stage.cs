using Astrum.Recruting.Domain.Aggregates.Candidate;
using Astrum.SharedLib.Domain.Entities;

namespace Astrum.Recruting.Domain.Aggregates.Board;

public class Stage : AggregateRootBase<Guid>
{
    public string Title { get; set; }
    public int Position { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public KanbanBoard KanbanBoard { get; set; }
    public Guid BoardId { get; set; }
    public List<StageCard> StageCards { get; set; }
}

public class StageCard : AggregateRootBase<Guid>
{
    public Guid StageId { get; set; }
    public Stage Stage { get; set; }
    public Guid HrmUserId { get; set; }
    public HrmUser HrmUser { get; set; }
    public List<StageEvent> CardEvents { get; set; }
}