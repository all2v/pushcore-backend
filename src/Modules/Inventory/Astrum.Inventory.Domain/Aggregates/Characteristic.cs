namespace Astrum.Inventory.Domain.Aggregates
{
    public class Characteristic
    {
        public Characteristic() { }
        public string Name { get; set; }
        public string? Value { get; set; }
        public bool IsCustomField { get; set; } = false;
    }
}