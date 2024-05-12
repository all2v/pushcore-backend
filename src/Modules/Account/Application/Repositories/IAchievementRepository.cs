using Astrum.Account.Aggregates;
using Astrum.SharedLib.Application.Contracts.Persistence.Repositories;

namespace Astrum.Account.Repositories;

public interface IAchievementRepository : IEntityRepository<Achievement, Guid>
{
}