using Astrum.SharedLib.Domain.Entities;

namespace Astrum.Recruting.Domain.Aggregates.Vacancy;

public class Vacancy : AggregateRootBase<Guid>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public long HhId { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset ExpiredAt { get; set; }
    public double Salary { get; set; }
    public string City { get; set; }
    public List<VacancyArea> VacancyAreas { get; set; }
    public List<VacancyType> VacancyTypes { get; set; }
    public List<VacancyRole> VacancyRoles { get; set; }
    public List<WorkType> WorkTypes { get; set; }
}

public enum ResponseState
{
}