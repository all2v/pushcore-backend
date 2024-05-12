using System.ComponentModel.DataAnnotations;
using Astrum.CodeRev.Domain.Aggregates.Enums;

namespace Astrum.CodeRev.Application.UserService.ViewModel.DTO.Review;

public class InterviewSolutionReview
{
    public string UserId { get; set; }
    [Required]
    public string InterviewSolutionId { get; set; }
    [Required]
    public string ReviewerComment { get; set; }
    [Required]
    public Grade AverageGrade { get; set; }
    [Required]
    public InterviewResult InterviewResult { get; set; }
    [Required]
    public IList<TaskSolutionReview> TaskSolutionsReviews { get; set; }
}