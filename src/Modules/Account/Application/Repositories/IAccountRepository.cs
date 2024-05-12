using Astrum.SharedLib.Application.Contracts.Persistence.Repositories;

namespace Astrum.Account.Repositories;

public interface IAccountRepository : IEntityRepository<Aggregates.Account, Guid>
{
}