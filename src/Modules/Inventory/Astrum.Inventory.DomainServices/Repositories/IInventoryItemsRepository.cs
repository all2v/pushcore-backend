using Astrum.Inventory.Domain.Aggregates;
using Astrum.SharedLib.Application.Contracts.Persistence.Repositories;

namespace Astrum.Inventory.DomainServices.Repositories
{
    public interface IInventoryItemsRepository : IEntityRepository<InventoryItem, Guid>
    {
    }
}
