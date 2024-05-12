using System.ComponentModel.DataAnnotations;
using Astrum.CodeRev.Application.UserService.Services.Interviews;
using Astrum.CodeRev.Application.UserService.Services.Tasks;
using Astrum.CodeRev.Application.UserService.ViewModel.DTO.Contest;
using Astrum.Infrastructure.Shared;
using Astrum.Infrastructure.Shared.Result.AspNetCore;
using Astrum.Logging.Entities;
using Astrum.Logging.Services;
using Astrum.SharedLib.Application.Helpers;
using Astrum.SharedLib.Common.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Astrum.CodeRev.Backoffice.UserService.Controllers;

[Area("CodeRev")]
[Route("[controller]")]
public class ContestController : ApiBaseController
{
    private readonly IInterviewService _interviewService;
    private readonly ITaskService _taskService;
    private readonly ILogHttpService _logger;

    public ContestController(IInterviewService interviewService, ITaskService taskService, ILogHttpService logger)
    {
        this._interviewService = interviewService;
        this._taskService = taskService;
        _logger = logger;
    }

    [HttpGet("i-sln-info")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result<InterviewSolutionInfo>))]
    [ProducesResponseType(typeof(InterviewSolutionInfo), StatusCodes.Status200OK)]
    public async Task<Result<InterviewSolutionInfo>> GetInterviewSolutionInfo()
    {
        var userId = JwtManager.GetUserIdFromRequest(Request);
        var interviewSolution = await _interviewService.GetInterviewSolutionInfo(userId.Value);
        _logger.Log(userId, interviewSolution, HttpContext, "Получена информация об интервью пользователя",
            TypeRequest.GET, ModuleAstrum.CodeRev);
        return interviewSolution;
    }

    [HttpPut("start-i-sln")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    public async Task<Result> StartInterviewSolution(
        [Required] [FromQuery(Name = "id")] string interviewSolutionId)
    {
        var result = await _interviewService.StartInterviewSolution(interviewSolutionId);
        _logger.Log(interviewSolutionId, result, HttpContext, "Начато интервью пользоватлея",
            TypeRequest.PUT, ModuleAstrum.CodeRev);
        return result;
    }

    [HttpPut("end-i-sln")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    public async Task<Result> EndInterviewSolution(
        [Required] [FromQuery(Name = "id")] string interviewSolutionId)
    {
        var result = await _interviewService.EndInterviewSolution(interviewSolutionId);
        _logger.Log(interviewSolutionId, result, HttpContext, "Закончено интервью пользоватлея",
            TypeRequest.PUT, ModuleAstrum.CodeRev);
        return result;
    }

    [HttpPut("end-task-sln")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    public async Task<Result> EndTaskSolution([Required] [FromQuery(Name = "id")] string taskSolutionId)
    {
        var result = await _taskService.EndTaskSolution(taskSolutionId);
        _logger.Log(taskSolutionId, result, HttpContext, "Задача завершена",
            TypeRequest.PUT, ModuleAstrum.CodeRev);
        return result;
    }

    [HttpGet("task-slns-info")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result<List<TaskSolutionInfoContest>>))]
    [ProducesResponseType(typeof(List<TaskSolutionInfoContest>), StatusCodes.Status200OK)]
    public async Task<Result<List<TaskSolutionInfoContest>>> GetTaskSolutionsInfo(
        [Required] [FromQuery(Name = "id")] string interviewSolutionId)
    {
        var result = await _taskService.GetTaskSolutionInfosForContest(interviewSolutionId);
        _logger.Log(interviewSolutionId, result, HttpContext, "Получена информация о задачах в интервью",
            TypeRequest.PUT, ModuleAstrum.CodeRev);
        return result;
    }
}