namespace Astrum.Inventory.Application.Models
{
    public class TemplateView
    {
        public Guid Id { get; set; }
        public DateTimeOffset? DateCreated { get; set; }
        public DateTimeOffset? DateModifed { get; set; }
        public string Title { get; set; }
        public string? LinkImage { get; set; }
        public Guid? PictureId { get; set; }
        public List<CharacteristicView> Characteristics { get; set; } = new();
    }
}