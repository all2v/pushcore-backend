using Astrum.SharedLib.Domain.Entities;

namespace Astrum.Recruting.Domain.Aggregates.Practice;

public class PracticeVacancy : AggregateRootBase<Guid>
{
    public string Name { get; set; }
    public Guid PracticeId { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset ExpiredAt { get; set; }
    public int MaxParticipantsCount { get; set; }
    public string TaskUrl { get; set; }
    public List<PracticeSkill> PracticeSkills { get; set; }
    public List<Responsibility> Responsibilities { get; set; }
    public List<PracticeRequest> PracticeRequests { get; set; }
}