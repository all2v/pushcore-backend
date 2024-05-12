using Astrum.Projects.Aggregates;
using Astrum.SharedLib.Application.Contracts.Persistence.Repositories;

namespace Astrum.Projects.Repositories
{
    public interface IProjectRepository : IEntityRepository<Project, Guid>
    {
    }
}
