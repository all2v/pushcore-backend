using Astrum.Inventory.Application.Models;
using Astrum.SharedLib.Common.CQS.Implementations;

namespace Astrum.Inventory.Application.Commands
{
    public class DeleteInventoryItemCommand : CommandResult<InventoryItemView>
    {
        public Guid InventoryItemId { get; set; }
        public DeleteInventoryItemCommand(Guid inventoryItemId)
        {
            InventoryItemId = inventoryItemId;
        }
    }
}
