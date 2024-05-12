using Astrum.SharedLib.Domain.Entities;

namespace Astrum.Recruting.Domain.Aggregates.Candidate;

public class HrmUser : AggregateRootBase<Guid>
{
    public string Name { get; set; }
    public string Srurname { get; set; }
    public string? LastName { get; set; }
    public Gender Gender { get; set; }
    public bool IsActive { get; set; }
    public Guid? ProfileId { get; set; }
    public string Email { get; set; }
    public string? TgLogin { get; set; }
    public List<CandidateResume> CandidateResumes { get; set; }
    public List<UserComment> UserComments { get; set; }
}

public enum Gender
{
    Male,
    Female,
    None
}