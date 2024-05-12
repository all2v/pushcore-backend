using Astrum.SharedLib.Application.Contracts.Persistence.Repositories;
using Astrum.Storage.Aggregates;

namespace Astrum.Storage.Repositories;

public interface IFileRepository : IEntityRepository<StorageFile, Guid>
{
}