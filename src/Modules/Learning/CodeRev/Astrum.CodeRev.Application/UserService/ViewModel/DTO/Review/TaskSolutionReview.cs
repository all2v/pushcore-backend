using System.ComponentModel.DataAnnotations;
using Astrum.CodeRev.Domain.Aggregates.Enums;

namespace Astrum.CodeRev.Application.UserService.ViewModel.DTO.Review;

public class TaskSolutionReview
{
    [Required]
    public string TaskSolutionId { get; set; }
    [Required]
    public Grade Grade { get; set; }
}