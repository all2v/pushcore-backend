using Astrum.Account.Aggregates;
using Astrum.SharedLib.Application.Contracts.Persistence.Repositories;

namespace Astrum.Account.Repositories;

public interface ITimelineRepository : IEntityRepository<AccessTimeline, Guid>
{
}