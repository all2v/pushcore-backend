using Astrum.Infrastructure.Shared;
using Astrum.Infrastructure.Shared.Result.AspNetCore;
using Astrum.Logging.Services;
using Astrum.Projects.Services;
using Astrum.Projects.ViewModels.Requests;
using Astrum.Projects.ViewModels.Views;
using Astrum.SharedLib.Common.Results;
using HotChocolate.Subscriptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Astrum.Projects.Controllers;

[Area("Projects")]
[Route("[area]/[controller]")]
public class CustomFieldController : ApiBaseController
{
    private readonly ICustomFieldService _customFieldService;
    private readonly ITopicEventSender _sender;
    private readonly ILogHttpService _logger;

    public CustomFieldController(ITopicEventSender sender, ICustomFieldService customFieldService, ILogHttpService logger)
    {
        _sender = sender;
        _customFieldService = customFieldService;
        _logger = logger;
    }

    /// <summary>
    ///     Создать кастомное поле
    /// </summary>
    [HttpPost]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(CustomFieldView), StatusCodes.Status200OK)]
    public async Task<Result<CustomFieldView>> Create([FromBody] CustomFieldRequest customFieldRequest)
    {
        var response = await _customFieldService.Create(customFieldRequest);
        _logger.Log(customFieldRequest, response, HttpContext, "Кастомное поле создано.", Logging.Entities.TypeRequest.POST, Logging.Entities.ModuleAstrum.Project);
        return response;
    }

    /// <summary>
    ///     Удалить кастомное поле
    /// </summary>
    [HttpDelete("{id}")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public async Task<Result<CustomFieldView>> Delete([FromRoute] Guid id)
    {
        var response =  await _customFieldService.Delete(id);
        _logger.Log(id, response, HttpContext, "Кастомное поле удалено.", Logging.Entities.TypeRequest.DELETE, Logging.Entities.ModuleAstrum.Project);
        return response;
    }
}