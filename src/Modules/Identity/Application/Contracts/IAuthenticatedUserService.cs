using Astrum.SharedLib.Domain.Enums;

namespace Astrum.Identity.Contracts;

public interface IAuthenticatedUserService
{
    Guid? UserId { get; }
    public string Username { get; }
    public string Name { get; }
    public IEnumerable<RolesEnum> Roles { get; }
}