using Astrum.CodeRev.Domain.Aggregates.Enums;

namespace Astrum.CodeRev.Application.UserService.ViewModel.DTO.Tasks;

public class TestTaskDto
{
    public Guid Id { get; set; }
    public string TaskText { get; set; }
    public string StartCode { get; set; }
    public string Name { get; set; }
    public string TestsCode { get; set; }
    public int RunAttempts { get; set; }
    public ProgrammingLanguage ProgrammingLanguage { get; set; }
}