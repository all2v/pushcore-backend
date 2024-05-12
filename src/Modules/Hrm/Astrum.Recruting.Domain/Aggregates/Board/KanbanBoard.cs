using Astrum.SharedLib.Domain.Entities;

namespace Astrum.Recruting.Domain.Aggregates.Board;

public class KanbanBoard : AggregateRootBase<Guid>
{
    public DateTimeOffset CreatedAt { get; set; }
    public Guid VacancyId { get; set; }
    public List<Stage> Stages { get; set; }
}