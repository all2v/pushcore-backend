using Astrum.SharedLib.Application.Contracts.Persistence.Repositories;

namespace Astrum.Appeal.Repositories;

public interface IAppealRepository : IEntityRepository<Aggregates.Appeal, Guid>
{
}