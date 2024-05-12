using Ardalis.Specification;
using Astrum.Inventory.Domain.Aggregates;
using Astrum.Inventory.DomainServices.Repositories;
using Astrum.SharedLib.Persistence.Repositories;

namespace Astrum.Inventory.Persistence.Repositories
{
    public class InventoryItemsRepository : EFRepository<InventoryItem, Guid, InventoryDbContext>, IInventoryItemsRepository
    {
        public InventoryItemsRepository(InventoryDbContext context, ISpecificationEvaluator? specificationEvaluator = null) : base(context, specificationEvaluator)
        {
        }
    }
}
