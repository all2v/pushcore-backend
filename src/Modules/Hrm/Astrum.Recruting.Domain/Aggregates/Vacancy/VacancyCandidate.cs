using Astrum.SharedLib.Domain.Entities;

namespace Astrum.Recruting.Domain.Aggregates.Vacancy;

public class VacancyCandidate : AggregateRootBase<long>
{
    public Vacancy Vacancy { get; set; }
    public Guid VacancyId { get; set; }
    public string CandidateResumeId { get; set; }
    public DateTimeOffset ResponseDate { get; set; }
    public ResponseState State { get; set; }
}