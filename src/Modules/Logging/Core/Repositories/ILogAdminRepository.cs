using Astrum.Logging.Entities;
using Astrum.SharedLib.Application.Contracts.Persistence.Repositories;

namespace Astrum.Logging.Repositories
{
    public interface ILogAdminRepository : IEntityRepository<LogAdmin, Guid>
    {
    }
}
