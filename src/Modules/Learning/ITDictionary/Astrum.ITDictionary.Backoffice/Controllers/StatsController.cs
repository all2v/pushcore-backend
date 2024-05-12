using Astrum.Infrastructure.Shared;
using Astrum.Infrastructure.Shared.Result.AspNetCore;
using Astrum.ITDictionary.Enums;
using Astrum.ITDictionary.Models.ViewModels;
using Astrum.ITDictionary.Services;
using Astrum.Logging.Services;
using Astrum.SharedLib.Common.Results;
using Microsoft.AspNetCore.Mvc;

namespace Astrum.ITDictionary.Controllers;

[Area("ITDictionary")]
[Route("[area]/[controller]")]
public class StatsController: ApiBaseController
{
    private readonly IStatisticsService _service;
    
    private readonly ILogHttpService _logger;

    public StatsController(IStatisticsService service, ILogHttpService logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Получить сводку 
    /// </summary>
    /// <param name="userId"></param>
    [HttpGet("{userId:guid}/summary")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(Result<StatisticsSummary>), 200)]
    public async Task<Result<StatisticsSummary>> GetSummary([FromRoute] Guid userId)
    {
        var response = await _service.GetSummary(userId);

        return response;
    }
    
    /// <summary>
    /// Получить статистику по тесту
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="type"></param>
    [HttpGet("{userId:guid}/tests")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(Result<TestStatisticsDetails>), 200)]
    public async Task<Result<TestStatisticsDetails>> GetTestStats([FromRoute] Guid userId, [FromQuery] PracticeType type)
    {
        var response = await _service.GetTestStatsWithDetails(userId, type);

        return response;
    }
}