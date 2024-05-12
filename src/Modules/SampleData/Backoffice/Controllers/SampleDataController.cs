using Astrum.Infrastructure.Shared;
using Astrum.Infrastructure.Shared.Result.AspNetCore;
using Astrum.SampleData.Services;
using Astrum.SharedLib.Common.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Astrum.SampleData.Controllers;

[Area("SampleData")]
[Route("[controller]")]
public class SampleDataController : ApiBaseController
{
    private readonly ISampleDataService _sampleDataService;
    public SampleDataController(ISampleDataService sampleDataService)
    {
        _sampleDataService = sampleDataService;
    }

    /// <summary>
    ///     reset data
    /// </summary>
    [HttpPost("reset")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public async Task<Result> Reset()
    {
        _sampleDataService.ResetToSampleData();
        return Result.Success();
    }
}
