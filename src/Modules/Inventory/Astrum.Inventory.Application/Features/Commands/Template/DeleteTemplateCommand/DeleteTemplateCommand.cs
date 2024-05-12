using Astrum.Inventory.Application.Models;
using Astrum.SharedLib.Common.CQS.Implementations;

namespace Astrum.Inventory.Application.Commands
{
    public class DeleteTemplateCommand : CommandResult<TemplateView>
    {
        public Guid TemplateId { get; set; }
        public DeleteTemplateCommand(Guid templateId)
        {
            TemplateId = templateId;
        }
    }
}
