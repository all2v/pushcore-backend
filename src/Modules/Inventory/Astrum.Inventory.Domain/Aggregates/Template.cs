using System.ComponentModel.DataAnnotations.Schema;
using Astrum.SharedLib.Domain.Entities;

namespace Astrum.Inventory.Domain.Aggregates
{
    public class Template : AggregateRootBase<Guid>
    {
        public Template() { }
        public Template(Guid id)
        {
            Id = id;
        }
        public string Title { get; set; }
        public Guid? PictureId { get; set;}
        [Column(TypeName = "jsonb")]
        public IEnumerable<Characteristic> Characteristics { get; set; }
    }
}