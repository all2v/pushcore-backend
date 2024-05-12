using Astrum.Infrastructure.Shared;
using Astrum.Infrastructure.Shared.Result.AspNetCore;
using Astrum.Inventory.Application.Commands;
using Astrum.Inventory.Application.Models;
using Astrum.Inventory.Application.Models.ViewModels;
using Astrum.Inventory.Application.Services;
using Astrum.Logging.Services;
using Astrum.SharedLib.Application.Models.Filters;
using Astrum.SharedLib.Common.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Astrum.Inventory.Backoffice.Controllers
{
    [Route("[controller]")]
    public class TemplatesController : ApiBaseController
    {
        private readonly ITemplatesService _service;
        private readonly ILogHttpService _logger;
        public TemplatesController(ITemplatesService service, ILogHttpService logger)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet("/api/inventory/get-templates")]
        [TranslateResultToActionResult]
        [ProducesDefaultResponseType(typeof(Result))]
        [ProducesResponseType(typeof(List<TemplateView>), StatusCodes.Status200OK)]
        public async Task<Result<List<TemplateView>>> Get()
        {
            var templates = await _service.GetTemplates();
            return Result.Success(templates);
        }

        [HttpGet("{id}")]
        [TranslateResultToActionResult]
        [ProducesDefaultResponseType(typeof(Result))]
        [ProducesResponseType(typeof(TemplateView), StatusCodes.Status200OK)]
        public async Task<Result<TemplateView>> GetById([FromRoute] Guid id)
        {
            var template = await _service.GetTemplateById(id);
            if (!template.IsSuccess)
            {
                _logger.Log(id, template, HttpContext, "Категория получена.", Logging.Entities.TypeRequest.GET, Logging.Entities.ModuleAstrum.Inventory);
            }
            return Result.Success(template);
        }

        [HttpPost]
        [TranslateResultToActionResult]
        [ProducesDefaultResponseType(typeof(Result))]
        [ProducesResponseType(typeof(TemplateView), StatusCodes.Status200OK)]
        public async Task<Result<TemplateView>> Create([FromBody] TemplateCreateRequest templateCreate)
        {
            var template = await _service.CreateTemplate(templateCreate);
            _logger.Log(templateCreate, template, HttpContext, "Категория создана.", Logging.Entities.TypeRequest.POST, Logging.Entities.ModuleAstrum.Inventory);
            return Result.Success(template);
        }

        [HttpPut("{id}")]
        [TranslateResultToActionResult]
        [ProducesDefaultResponseType(typeof(Result))]
        [ProducesResponseType(typeof(TemplateView), StatusCodes.Status200OK)]
        public async Task<Result<TemplateView>> Update([FromRoute] Guid id, [FromBody]TemplateUpdateRequest templateUpdate)
        {
            var command = new UpdateTemplateCommand(id, templateUpdate);
            var template = await Mediator.Send(command);
            _logger.Log(templateUpdate, template, HttpContext, "Категория обновлена.", Logging.Entities.TypeRequest.PUT, Logging.Entities.ModuleAstrum.Inventory);
            return template;
        }

        [HttpDelete("{id}")]
        [TranslateResultToActionResult]
        [ProducesDefaultResponseType(typeof(Result))]
        [ProducesResponseType(typeof(TemplateView), StatusCodes.Status200OK)]
        public async Task<Result<TemplateView>> Delete([FromRoute] Guid id)
        {
            var command = new DeleteTemplateCommand(id);
            var template = await Mediator.Send(command);
            _logger.Log(id, template, HttpContext, "Категория удалена.", Logging.Entities.TypeRequest.DELETE, Logging.Entities.ModuleAstrum.Inventory);
            return template;
        }

        [TranslateResultToActionResult]
        [HttpGet("/api/inventory/get-filter-info")]
        [ProducesDefaultResponseType(typeof(Result))]
        [ProducesResponseType(typeof(FilterInfo), StatusCodes.Status200OK)]
        public async Task<Result<FilterInfo>> GetFilterInfo()
        {
            var templates = await _service.GetFilterInfo();
            return Result.Success(templates.Data);
        }

        /// <summary>
        ///     Получение фильтров
        /// </summary>
        [HttpGet("filters")]
        [TranslateResultToActionResult]
        [ProducesResponseType(typeof(List<FilterResponse>), StatusCodes.Status200OK)]
        public async Task<Result<FilterResponse>> GetFilters()
        {
            var res = await _service.GetFilters();
            return res;
        }
    }
}
