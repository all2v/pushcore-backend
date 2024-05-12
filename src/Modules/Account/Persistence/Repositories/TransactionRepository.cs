using Ardalis.Specification;
using Astrum.Account.Application.Repositories;
using Astrum.Account.Domain.Aggregates;
using Astrum.SharedLib.Persistence.Repositories;

namespace Astrum.Account.Persistence.Repositories
{
    public class TransactionRepository : EFRepository<Transaction, Guid, AccountDbContext>,
        ITransactionRepository
    {
        public TransactionRepository(AccountDbContext context, ISpecificationEvaluator? specificationEvaluator = null) :
            base(context, specificationEvaluator)
        {
        }
    }
}
