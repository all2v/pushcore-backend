using Astrum.SharedLib.Domain.Entities;

namespace Astrum.Recruting.Domain.Aggregates.Board;

public class StageEvent : AggregateRootBase<Guid>
{
    public string Name { get; set; }
    public DateTimeOffset EventDateTime { get; set; }
    public Guid AccountableId { get; set; }
    public StageCard StageCard { get; set; }
    public Guid StageCardId { get; set; }
    public Guid StageId { get; set; }
}