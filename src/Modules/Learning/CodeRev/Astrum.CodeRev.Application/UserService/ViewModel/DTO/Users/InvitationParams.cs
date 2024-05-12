using System.ComponentModel.DataAnnotations;

namespace Astrum.CodeRev.Application.UserService.ViewModel.DTO.Users;

public class InvitationParams
{
    [Required]
    public string Role { get; set; }
    public string InterviewId { get; set; }
    public bool IsSynchronous { get; set; }
}