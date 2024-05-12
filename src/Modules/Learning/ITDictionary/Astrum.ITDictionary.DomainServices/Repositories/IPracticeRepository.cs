using Astrum.ITDictionary.Aggregates;
using Astrum.SharedLib.Application.Contracts.Persistence.Repositories;

namespace Astrum.ITDictionary.Repositories;

public interface IPracticeRepository : IEntityRepository<Practice, Guid>
{
}