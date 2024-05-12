
using Astrum.CodeRev.Domain.Aggregates.Enums;

namespace Astrum.CodeRev.Application.UserService.ViewModel.DTO.Review;

public class TaskSolutionInfo
{
    public Guid TaskSolutionId { get; set; }
    public Guid TaskId { get; set; }
    public Guid InterviewSolutionId { get; set; }
    public string FullName { get; set; }
    public char TaskOrder { get; set; }
    public bool IsDone { get; set; }
    public Grade Grade { get; set; }
    public int RunAttemptsLeft { get; set; }
    public ProgrammingLanguage ProgrammingLanguage { get; set; }
}