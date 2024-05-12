using Astrum.Identity.Models;

namespace Astrum.Identity;

public interface IUserContextAccessor
{
    Task<ApplicationUser?> GetUser();
}