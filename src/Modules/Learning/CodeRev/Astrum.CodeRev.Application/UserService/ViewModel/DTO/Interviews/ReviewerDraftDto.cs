using System.ComponentModel.DataAnnotations;
using Astrum.CodeRev.Domain.Aggregates.Draft;

namespace Astrum.CodeRev.Application.UserService.ViewModel.DTO.Interviews;

public class ReviewerDraftDto
{
    [Required]
    public string InterviewSolutionId { get; set; }
    [Required]
    public Draft Draft { get; set; }
}