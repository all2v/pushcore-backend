namespace Astrum.Identity.DTOs;

/// <summary>
///     Represents a request to add/remove a role to/from a user
/// </summary>
public record RoleAssignmentRequestDto(string Username, List<string> Roles)
{
    public string Username { get; set; } = Username;
    public List<string> Roles { get; set; } = Roles;
}