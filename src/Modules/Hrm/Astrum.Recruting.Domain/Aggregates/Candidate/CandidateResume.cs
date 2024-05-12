using Astrum.Recruting.Domain.Aggregates.Candidate;
using Astrum.SharedLib.Domain.Entities;

namespace Astrum.Recruting.Domain.Aggregates;

public class CandidateResume : AggregateRootBase<string>
{
    public string Name { get; set; }
    public string Srurname { get; set; }
    public string? LastName { get; set; }
    public Gender Gender { get; set; }
    public bool IsActive { get; set; }
    public Guid? ProfileId { get; set; }
    public string Email { get; set; }
    public string About { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public double Salary { get; set; }
    public HrmUser HrmUser { get; set; }
    public Guid HrmUserId { get; set; }
    public List<JobExperience> JobExperiences { get; set; }
    
}