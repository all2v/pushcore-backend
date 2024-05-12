using Astrum.Account.Application.ViewModels;
using Astrum.Identity.Models;
using Astrum.SharedLib.Domain.Enums;

namespace Astrum.Account.Features.Profile;

public class UserProfileSummary
{
    public Guid UserId { get; set; }
    public string Username { get; set; }
    public Guid? AvatarImageId { get; set; }
    public string AvatarUrl { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Patronymic { get; set; }
    public string NameWithSurname 
    { 
        get
        {
            return $"{Surname} {Name}";
        }
    }
    public IEnumerable<RolesEnum> Roles { get; set; }
    //public List<RoleForm> Roles { get; set; } = new();
    public Guid? PositionId { get; set; }
    public string? PositionName { get; set; }
    public string Email { get; set; }
    public DateTime? BirthDate { get; set; }
    public string? Address { get; set; }
    public string PrimaryPhone { get; set; }
    public string SecondaryPhone { get; set; }
    public bool IsActive { get; set; }
    public SocialNetworksResponse SocialNetworks { get; set; }
    public string[] Competencies { get; set; }
    public double Money { get; set; }
    public string Role { get; set; }
}