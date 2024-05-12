using Astrum.Debts.Domain.Aggregates;
using Astrum.SharedLib.Application.Contracts.Persistence.Repositories;

namespace Astrum.Debts.DomainServices.Repositories
{
    public interface IDebtsRepository : IEntityRepository<Debt, Guid>
    {
    }
}
