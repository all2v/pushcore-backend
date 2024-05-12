
using Astrum.CodeRev.Domain.Aggregates.Enums;

namespace Astrum.CodeRev.Application.UserService.ViewModel.DTO.Review;

public class InterviewSolutionDto
{
    public Guid InterviewSolutionId { get; set; }
    public string Username { get; set; }
    public Guid InterviewId { get; set; }
    public string FullName { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string Vacancy { get; set; }
    public long StartTimeMs { get; set; }
    public long EndTimeMs { get; set; }
    public long TimeToCheckMs { get; set; } // fixed time until which solution must be checked
    public string ReviewerComment { get; set; }
    public Grade AverageGrade { get; set; }
    public InterviewResult InterviewResult { get; set; }
    public bool IsSubmittedByCandidate { get; set; }
    public List<ProgrammingLanguage> ProgrammingLanguages { get; set; }
    public List<TaskSolutionInfo> TaskSolutionsInfos { get; set; }
}