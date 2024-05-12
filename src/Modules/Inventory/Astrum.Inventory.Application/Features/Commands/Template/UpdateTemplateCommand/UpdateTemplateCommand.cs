using Astrum.Inventory.Application.Models;
using Astrum.SharedLib.Common.CQS.Implementations;

namespace Astrum.Inventory.Application.Commands
{
    public class UpdateTemplateCommand : CommandResult<TemplateView>
    {
        public Guid TemplateId { get; set; }
        public TemplateUpdateRequest Template { get; set; }
        public UpdateTemplateCommand(Guid templateId, TemplateUpdateRequest template)
        {
            TemplateId = templateId;
            Template = template;
        }
    }
}
