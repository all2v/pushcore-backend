using Astrum.Account.Domain.Aggregates;
using Astrum.SharedLib.Application.Contracts.Persistence.Repositories;

namespace Astrum.Account.Application.Repositories
{
    public interface ITransactionRepository : IEntityRepository<Transaction, Guid>
    {
    }
}
