using Astrum.Identity.Models;

namespace Astrum.Identity;

public interface IApplicationUserAccessor
{
    Task<ApplicationUser?> GetUser();
}