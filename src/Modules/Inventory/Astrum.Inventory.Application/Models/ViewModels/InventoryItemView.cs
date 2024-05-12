using Astrum.Inventory.Domain.Aggregates;
using Astrum.Storage.ViewModels;

namespace Astrum.Inventory.Application.Models
{
    public class InventoryItemView
    {
        public Guid Id { get; set; }
        public DateTimeOffset? DateCreated { get; set; }
        public DateTimeOffset? DateModifed { get; set; }
        public string Model { get; set; }
        public bool IsPublic { get; set; } = true;
        public string SerialNumber { get; set; }
        public Status Status { get; set; }
        public Guid? TemplateId { get; set; }
        public Template? Template { get; set; }
        public int State { get; set; }
        public UserInventory? User { get; set; }
        public string? LinkImage { get; set; }
        public Guid? PictureId { get; set; }
        public Guid? UserId { get; set; }
        public List<CharacteristicView>? Characteristics { get; set; } = new();
        public int? Index { get; set; }
    }
}
