using Astrum.Identity.Models;
using Astrum.SharedLib.Persistence.Repositories;

namespace Astrum.Identity.Repositories.EntityRepositories;

public class ApplicationUserRepository : EFRepository<ApplicationUser, Guid, IdentityDbContext>,
    IApplicationUserRepository
{
    public ApplicationUserRepository(IdentityDbContext context) : base(context)
    {
    }
}