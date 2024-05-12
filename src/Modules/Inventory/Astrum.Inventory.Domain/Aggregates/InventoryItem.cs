using System.ComponentModel.DataAnnotations.Schema;
using Astrum.SharedLib.Domain.Entities;

namespace Astrum.Inventory.Domain.Aggregates
{
    public class InventoryItem : AggregateRootBase<Guid>
    {
        public InventoryItem() { }
        public InventoryItem(Guid id)
        {
            Id = id;
        }
        public string Model { get; set; }
        //TODO: Возможно придётся убрать IsPublic
        public bool IsPublic { get; set; } = true;
        public string SerialNumber { get; set; }
        public Status Status { get; set; }
        public int State { get; set; }
        public Guid? UserId { get; set; }
        public Guid? PictureId { get; set; }
        public Template? Template { get; set; }
        public Guid? TemplateId { get; set; }
        public int Index { get; set; }
        [Column(TypeName = "jsonb")]
        public IEnumerable<Characteristic>? Characteristics { get; set; }
    }
}