using Astrum.Inventory.Application.Models;
using Astrum.Inventory.Application.Models.ViewModels;
using Astrum.Inventory.Domain.Aggregates;
using Astrum.SharedLib.Common.Results;

namespace Astrum.Inventory.Application.Services
{
    public interface IInventoryItemsService
    {
        Task<Result<List<InventoryItemView>>> GetInventoryItems(CancellationToken cancellationToken = default);
        Task<Result<InventoryItemsPaginationView>> GetFilteringInventoryItems(string[]? templates, string? predicate, Status[]? statuses, Guid? userId,
            int? startIndex, int? count, CancellationToken cancellationToken = default);
        Task<Result<InventoryItemView>> GetInventoryItemById(Guid id, CancellationToken cancellationToken = default);
        Task<Result<InventoryItemView>> Create(InventoryItemCreateRequest inventoryItemCreate, CancellationToken cancellationToken = default);
        Task<Result<InventoryItemView>> Update(Guid id, InventoryItemUpdateRequest inventoryItemUpdate, CancellationToken cancellationToken = default);
        Task<Result<InventoryItemView>> Delete(Guid id, CancellationToken cancellationToken = default);
    }
}
