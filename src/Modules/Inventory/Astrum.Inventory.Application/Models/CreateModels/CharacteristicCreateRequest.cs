using Astrum.Inventory.Domain.Aggregates;

namespace Astrum.Inventory.Application.Models
{
    public  class CharacteristicCreateRequest
    {
        public string Name { get; set; }
        public string? Value { get; set; }
        public bool IsCustomField { get; set; } = false;
    }
}
