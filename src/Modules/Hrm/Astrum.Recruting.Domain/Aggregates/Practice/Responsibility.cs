using Astrum.SharedLib.Domain.Entities;

namespace Astrum.Recruting.Domain.Aggregates.Practice;

public class Responsibility : AggregateRootBase<Guid>
{
    public string Name { get; set; }
    public PracticeVacancy PracticeVacancy { get; set; }
    public Guid PracticeVacancyId { get; set; }
}