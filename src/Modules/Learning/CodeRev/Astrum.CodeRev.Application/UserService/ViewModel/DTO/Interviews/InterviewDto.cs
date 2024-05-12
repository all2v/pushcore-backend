
using Astrum.CodeRev.Domain.Aggregates.Enums;

namespace Astrum.CodeRev.Application.UserService.ViewModel.DTO.Interviews;

public class InterviewDto
{
    public Guid Id { get; set; }
    public string Vacancy { get; set; }
    public string InterviewText { get; set; }
    public long InterviewDurationMs { get; set; }
    public string CreatedByUsername { get; set; }
    public List<ProgrammingLanguage> InterviewLanguages { get; set; }
}