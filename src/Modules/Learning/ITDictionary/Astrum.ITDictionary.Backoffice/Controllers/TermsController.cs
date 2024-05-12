using Astrum.Infrastructure.Shared;
using Astrum.Infrastructure.Shared.Result.AspNetCore;
using Astrum.ITDictionary.Models.Requests;
using Astrum.ITDictionary.Models.ViewModels;
using Astrum.ITDictionary.Services;
using Astrum.Logging.Services;
using Astrum.SharedLib.Common.Results;
using Microsoft.AspNetCore.Mvc;

namespace Astrum.ITDictionary.Controllers;

[Area("ITDictionary")]
[Route("[area]/[controller]")]
public class TermsController: ApiBaseController
{
    private readonly ILogHttpService _logger;
    private readonly ITermService _termService;

    public TermsController(ILogHttpService logger, ITermService termService)
    {
        _logger = logger;
        _termService = termService;
    }

    
    /// <summary>
    /// Получить термин по id
    /// </summary>
    /// <param name="id"></param>
    [HttpGet("{id:guid}")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(Result<TermView>), 200)]
    public async Task<Result<TermView>> GetById([FromRoute] Guid id)
    {
        var response = await _termService.GetByIdAsync(id);
        return response;
    }

    /// <summary>
    /// Создать термин
    /// </summary>
    /// <param name="request"></param>
    [HttpPost]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(Result<TermView>), 200)]
    public async Task<Result<TermView>> Create([FromBody] TermRequest request)
    {
        var response = await _termService.CreateAsync(request);
        return response;
    }

    [HttpDelete("{id:guid}")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    public async Task<Result> Delete([FromRoute] Guid id)
    {
        var response = await _termService.DeleteAsync(id);
        return response;
    }

    /// <summary>
    /// Получить список терминов
    /// </summary>
    [HttpGet]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(List<TermView>), 200)]
    public async Task<Result<List<TermView>>> GetAll()
    {
        var response = await _termService.GetAllAsync();
        return response;
    }

    /// <summary>
    /// Получить термины по id категории
    /// </summary>
    /// <param name="id"></param>
    [HttpGet("categories/{id:guid}")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(List<TermView>), 200)]
    public async Task<Result<List<TermView>>> GetByCategory([FromRoute] Guid id)
    {
        var response = await _termService.GetByCategoryIdAsync(id);
        if (response.Failed)
        {
        }

        return response;
    }

    /// <summary>
    /// Поиск терминов
    /// </summary>
    /// <param name="substring"></param>
    [HttpGet("search")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(List<TermView>), 200)]
    public async Task<Result<List<TermView>>> SearchTerms([FromQuery] string substring)
    {
        var response = await _termService.SearchAsync(substring);
        return response;
    }
}