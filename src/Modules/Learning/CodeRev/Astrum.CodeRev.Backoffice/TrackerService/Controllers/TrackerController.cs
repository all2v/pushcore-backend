using System.Text.Json.Nodes;
using Astrum.CodeRev.Application.TrackerService.Services;
using Astrum.CodeRev.Application.TrackerService.ViewModels.DTO;
using Astrum.Infrastructure.Shared;
using Astrum.Infrastructure.Shared.Result.AspNetCore;
using Astrum.Logging.Entities;
using Astrum.Logging.Services;
using Astrum.SharedLib.Common.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace Astrum.CodeRev.Backoffice.TrackerService.Controllers;

[Area("CodeRev")]
[Route("v1.0/tracker")]
public class TrackerController : ApiBaseController
{
    private readonly ITaskRecordDeserializer _deserializer;
    private readonly ITaskRecordSerializer _serializer;
    private readonly ITrackerService _trackerService;
    private readonly ILogHttpService _logger;


    public TrackerController(ITaskRecordDeserializer deserializer, ITaskRecordSerializer serializer,
        ITrackerService trackerService, ILogHttpService logger)
    {
        _deserializer = deserializer;
        _serializer = serializer;
        _trackerService = trackerService;
        _logger = logger;
    }

    [HttpGet("get")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result<List<RecordChunkResponseDto>>))]
    [ProducesResponseType(typeof(List<RecordChunkResponseDto>), StatusCodes.Status200OK)]
    public async Task<Result<List<RecordChunkResponseDto>>> Get([FromQuery] Guid taskSolutionId,
        [FromQuery] decimal? saveTime)
    {
        var result = await _trackerService.Get(taskSolutionId, saveTime);
        if (!result.IsSuccess)
            return Result.Error(result.MessageWithErrors);
        var response = _serializer.Serialize(result);
        _logger.Log(saveTime, result, HttpContext, "Получены записи написания кода",
            TypeRequest.GET, ModuleAstrum.CodeRev);
        return Result.Success(response);
    }

    [HttpGet("get-last-code")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result<LastCodeDto>))]
    [ProducesResponseType(typeof(LastCodeDto), StatusCodes.Status200OK)]
    public async Task<Result<LastCodeDto?>> GetLastCode([FromQuery] Guid taskSolutionId)
    {
        var result = await _trackerService.GetLastCode(taskSolutionId);
        _logger.Log(taskSolutionId, result, HttpContext, "Получена последняя версия кода",
            TypeRequest.GET, ModuleAstrum.CodeRev);
        return result;
    }

    [HttpPut("save")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public async Task<Result> Save([FromBody] JObject requestDto)
    {
        var request = new TaskRecordRequestDto
        {
            Code = requestDto.GetValue("code").ToString(),
            SaveTime = decimal.Parse(requestDto.GetValue("saveTime").ToString()),
            TaskSolutionId = Guid.Parse(requestDto.GetValue("taskSolutionId").ToString()),
            Records = requestDto.GetValue("records").Values<JToken>().Select(token => JObject.Parse(token.ToString()))
                .ToList()
        };

        var taskRecord = _deserializer.ParseRequestDto(request);
        var result = await _trackerService.Create(taskRecord);
        _logger.Log(requestDto, result, HttpContext, "Создана запись о написанном коде",
            TypeRequest.POST, ModuleAstrum.CodeRev);
        return result;
    }

    [HttpPost("save-video")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public async Task<Result> SaveVideo([FromBody] JsonValue obj)
    {
        Console.WriteLine(obj);
        return Result.Success();
    }
}