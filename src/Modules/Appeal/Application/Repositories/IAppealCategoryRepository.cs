using Astrum.Appeal.Aggregates;
using Astrum.SharedLib.Application.Contracts.Persistence.Repositories;

namespace Astrum.Appeal.Repositories;

public interface IAppealCategoryRepository : IEntityRepository<AppealCategory, Guid>
{
}