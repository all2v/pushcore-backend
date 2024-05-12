using Astrum.SharedLib.Application.Contracts.Persistence.Repositories;

namespace Astrum.Example.Repositories;

public interface IExampleRepository : IEntityRepository<Aggregates.Example, Guid>
{
}