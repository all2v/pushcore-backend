using System.ComponentModel.DataAnnotations;

namespace Astrum.CodeRev.Application.UserService.ViewModel.DTO.Auth;

public class UserInvite
{
    [Required] public string FirstName { get; set; }
    [Required] public string Surname { get; set; }
    [Required] public string Username { get; set; }

    [Required] [Phone] public string PhoneNumber { get; set; }

    [Required] public string Email { get; set; }
}