using Ardalis.Specification;
using Astrum.Inventory.Domain.Aggregates;
using Astrum.Inventory.DomainServices.Repositories;
using Astrum.SharedLib.Persistence.Repositories;

namespace Astrum.Inventory.Persistence.Repositories
{
    public class TemplatesRepository : EFRepository<Template, Guid, InventoryDbContext>, ITemplatesRepository
    {
        public TemplatesRepository(InventoryDbContext context, ISpecificationEvaluator? specificationEvaluator = null) : base(context, specificationEvaluator)
        {
        }
    }
}
