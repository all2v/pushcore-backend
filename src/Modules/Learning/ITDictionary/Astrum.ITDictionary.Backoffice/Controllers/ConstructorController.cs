using Astrum.Infrastructure.Shared;
using Astrum.Infrastructure.Shared.Result.AspNetCore;
using Astrum.ITDictionary.Models.Requests;
using Astrum.ITDictionary.Models.ViewModels;
using Astrum.ITDictionary.Services;
using Astrum.SharedLib.Common.Results;
using Microsoft.AspNetCore.Mvc;

namespace Astrum.ITDictionary.Controllers;


[Area("ITDictionary")]
[Route("[area]/[controller]")]
public class ConstructorController: ApiBaseController
{
    private readonly ITermConstructorService _service;
    private readonly ITermService _termService;

    public ConstructorController(ITermConstructorService service, ITermService termService)
    {
        _service = service;
        _termService = termService;
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
    /// Получить список терминов, выбранных пользователем
    /// </summary>
    /// <param name="userId"></param>
    [HttpGet("{userId:guid}/selected")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(Result<List<TermView>>), 200)]
    public async Task<Result<List<TermView>>> GetUserTerms([FromRoute] Guid userId)
    {
        var result = await _service.GetSelectedTermsResult(userId);

        return result;
    }

    
    [HttpPost]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    public async Task<Result> UpdateSelectedTerms([FromBody] UserTermsRequest request)
    {
        var result = await _service.UpdateUserTerms(request);
        return result;
    }
}