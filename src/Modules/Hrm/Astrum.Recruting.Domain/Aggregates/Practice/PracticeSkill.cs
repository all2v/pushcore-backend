using Astrum.SharedLib.Domain.Entities;

namespace Astrum.Recruting.Domain.Aggregates.Practice;

public class PracticeSkill : AggregateRootBase<Guid>
{
    public string Name { get; set; }
    public List<PracticeVacancy> PracticeVacancies { get; set; }
}