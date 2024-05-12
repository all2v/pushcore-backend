using Ardalis.Specification;
using Astrum.Debts.Domain.Aggregates;
using Astrum.Debts.DomainServices.Repositories;
using Astrum.SharedLib.Persistence.Repositories;

namespace Astrum.Debts.Persistence.Repositories
{
    public class DebtsRepository : EFRepository<Debt, Guid, DebtsDbContext>, IDebtsRepository
    {
        public DebtsRepository(DebtsDbContext context, ISpecificationEvaluator? specificationEvaluator = null) : base(context, specificationEvaluator)
        {
        }
    }
}
