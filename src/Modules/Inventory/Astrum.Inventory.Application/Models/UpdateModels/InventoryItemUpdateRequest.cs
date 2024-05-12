using Astrum.Inventory.Domain.Aggregates;
using Astrum.Storage.ViewModels;

namespace Astrum.Inventory.Application.Models
{
    public class InventoryItemUpdateRequest
    {
        public string Model { get; set; }
        public bool IsPublic { get; set; } = true;
        public string SerialNumber { get; set; }
        public Status Status { get; set; }
        public Guid? UserId { get; set; }
        public FileForm? Image { get; set; }
        public int State { get; set; }
        public Guid? TemplateId { get; set; }
        public List<CharacteristicUpdateRequest>? Characteristics { get; set; }

    }
}
