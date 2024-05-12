using Astrum.Infrastructure.Shared;
using Astrum.Infrastructure.Shared.Result.AspNetCore;
using Astrum.SampleData.Models;
using Astrum.SampleData.Services;
using Astrum.SharedLib.Common.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Astrum.SampleData.Controllers;

[Area("SampleData")]
[Route("[controller]")]
public class SampleContentController : ApiBaseController
{
    private readonly ISampleContentService _sampleContentService;
    public SampleContentController(ISampleContentService sampleContentService)
    {
        _sampleContentService = sampleContentService;
    }
    /// <summary>
    ///     Добавить файл контекста
    /// </summary>
    [HttpPost]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public async Task<Result> Add([FromForm] SampleContentDTO sampleContent)
    {
        return await _sampleContentService.Create(sampleContent);
    }
}
