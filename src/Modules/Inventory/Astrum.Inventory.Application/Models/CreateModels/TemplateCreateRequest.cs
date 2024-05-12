using Astrum.Storage.ViewModels;

namespace Astrum.Inventory.Application.Models
{
    public class TemplateCreateRequest
    {
        public string Title { get; set; }
        public List<CharacteristicCreateRequest>? Characteristics { get; set; } = new();
        public FileForm? Image { get; set; }
    }
}
