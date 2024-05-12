using System.ComponentModel.DataAnnotations;

namespace Astrum.CodeRev.Application.UserService.ViewModel.DTO.Auth;

public class LoginRequest
{
    [Required]
    // [EmailAddress]
    public string Email { get; set; }
    [Required]
    public string PasswordHash { get; set; }
}