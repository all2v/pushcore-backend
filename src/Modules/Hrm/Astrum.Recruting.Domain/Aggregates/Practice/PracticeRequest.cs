using Astrum.SharedLib.Domain.Entities;

namespace Astrum.Recruting.Domain.Aggregates.Practice;

public class PracticeRequest : AggregateRootBase<Guid>
{
    public string Fio { get; set; }
    public string TgLogin { get; set; }
    public string TaskUrl { get; set; }
    
    public bool IsApproved { get; set; }
    public string About { get; set; }
    public Guid HrmUserId { get; set; }
    public PracticeVacancy PracticeVacancy { get; set; }
    public Guid PracticeVacancyId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}