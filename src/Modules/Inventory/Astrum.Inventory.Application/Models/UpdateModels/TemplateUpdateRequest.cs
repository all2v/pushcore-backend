
using Astrum.Storage.ViewModels;

namespace Astrum.Inventory.Application.Models
{
    public class TemplateUpdateRequest
    {
        public string? Title { get; set; }
        public List<CharacteristicUpdateRequest>? Characteristics { get; set; }
        public FileForm? Image { get; set; }
    }
}
