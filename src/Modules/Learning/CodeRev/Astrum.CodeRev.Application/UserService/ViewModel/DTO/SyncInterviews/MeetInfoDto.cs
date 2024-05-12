using Astrum.CodeRev.Domain.Aggregates.Enums;

namespace Astrum.CodeRev.Application.UserService.ViewModel.DTO.SyncInterviews;

public class MeetInfoDto
{
    public Guid UserId { get; set; }
    public Guid InterviewSolutionId { get; set; }
    public Guid InterviewId { get; set; }
    public string FirstName { get; set; }
    public string Surname { get; set; }
    public string Vacancy { get; set; }
    public int TasksCount { get; set; }
    public List<ProgrammingLanguage> ProgrammingLanguages { get; set; }
    public bool IsOwnerMeet { get; set; }
}