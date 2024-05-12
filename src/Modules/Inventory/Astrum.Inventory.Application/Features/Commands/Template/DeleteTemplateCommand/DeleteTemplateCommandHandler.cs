using Astrum.Inventory.Application.Models;
using Astrum.Inventory.Application.Services;
using Astrum.SharedLib.Common.CQS.Implementations;
using Astrum.SharedLib.Common.Results;
using AutoMapper;

namespace Astrum.Inventory.Application.Commands
{
    public class DeleteTemplateCommandHandler : CommandResultHandler<DeleteTemplateCommand, TemplateView>
    {
        private readonly ITemplatesService _service;
        private readonly IMapper _mapper;
        public DeleteTemplateCommandHandler(ITemplatesService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public async override Task<Result<TemplateView>> Handle(DeleteTemplateCommand command, CancellationToken cancellationToken = default)
        {
            var template = await _service.GetTemplateById(command.TemplateId);
            if (template == null)
            {
                Result.NotFound("Шаблон не найден");
            }
            var response = await _service.DeleteTemplate(command.TemplateId);
            return Result.Success(response);
        }
    }
}
