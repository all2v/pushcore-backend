using Astrum.SharedLib.Domain.Entities;

namespace Astrum.Recruting.Domain.Aggregates.Candidate;

public class UserComment : AggregateRootBase<Guid>
{
    public DateTimeOffset CreatedAt { get; set; }
    public string Comment { get; set; }
    public HrmUser HrmUser { get; set; }
    public Guid HrmUserId { get; set; }
    public Guid? StageId { get; set; }
    public Guid? VacancyId { get; set; }
}