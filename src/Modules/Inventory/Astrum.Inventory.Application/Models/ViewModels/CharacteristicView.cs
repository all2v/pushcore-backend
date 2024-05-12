using Astrum.Inventory.Domain.Aggregates;

namespace Astrum.Inventory.Application.Models
{
    public class CharacteristicView
    {
        public string Name { get; set; }
        public string? Value { get; set; }
        public bool IsCustomField { get; set; } = false;
    }
}
