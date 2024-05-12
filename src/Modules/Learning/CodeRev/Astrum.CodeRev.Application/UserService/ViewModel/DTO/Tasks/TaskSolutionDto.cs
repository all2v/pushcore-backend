using Astrum.CodeRev.Domain.Aggregates.Enums;

namespace Astrum.CodeRev.Application.UserService.ViewModel.DTO.Tasks;

public class TaskSolutionDto
{
    public Guid Id { get; set; }
    public Guid TaskId { get; set; }
    public Guid InterviewSolutionId { get; set; }
    public bool IsDone { get; set; }
    public Grade Grade { get; set; }
    public int RunAttemptsLeft { get; set; }
}