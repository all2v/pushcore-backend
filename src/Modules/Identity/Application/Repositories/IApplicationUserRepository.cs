using Astrum.Identity.Models;
using Astrum.SharedLib.Application.Contracts.Persistence.Repositories;

namespace Astrum.Identity.Repositories;

public interface IApplicationUserRepository : IEntityRepository<ApplicationUser, Guid>
{
}