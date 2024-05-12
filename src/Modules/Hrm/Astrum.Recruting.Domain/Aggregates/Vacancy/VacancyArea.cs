using Astrum.SharedLib.Domain.Entities;

namespace Astrum.Recruting.Domain.Aggregates.Vacancy;

public class VacancyArea : AggregateRootBase<long>
{
    public string Name { get; set; }
    public List<Vacancy> Vacancies { get; set; }
}