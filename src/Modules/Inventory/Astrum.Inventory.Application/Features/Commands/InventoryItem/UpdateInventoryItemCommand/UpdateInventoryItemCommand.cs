using Astrum.Inventory.Application.Models;
using Astrum.SharedLib.Common.CQS.Implementations;

namespace Astrum.Inventory.Application.Commands
{
    public class UpdateInventoryItemCommand : CommandResult<InventoryItemView>
    {
        public Guid InventoryItemId { get; set; }
        public InventoryItemUpdateRequest InventoryItem { get; set; }
        public UpdateInventoryItemCommand(Guid inventoryItemId, InventoryItemUpdateRequest inventoryItem)
        {
            InventoryItemId = inventoryItemId;
            InventoryItem = inventoryItem;
        }
    }
}
