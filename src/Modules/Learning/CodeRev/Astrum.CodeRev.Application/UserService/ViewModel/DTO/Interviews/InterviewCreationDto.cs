using System.ComponentModel.DataAnnotations;

namespace Astrum.CodeRev.Application.UserService.ViewModel.DTO.Interviews;

public class InterviewCreationDto
{
    [Required]
    public string Vacancy { get; set; }
    [Required]
    public string InterviewText { get; set; }
    [Required]
    public long InterviewDurationMs { get; set; }
    [Required]
    public List<Guid> TaskIds { get; set; }
}