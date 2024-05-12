using System.ComponentModel.DataAnnotations;
using Astrum.CodeRev.Application.UserService.Services.Tasks;
using Astrum.CodeRev.Application.UserService.ViewModel.DTO;
using Astrum.CodeRev.Application.UserService.ViewModel.DTO.Review;
using Astrum.CodeRev.Application.UserService.ViewModel.DTO.Tasks;
using Astrum.CodeRev.Domain.Aggregates.Enums;
using Astrum.Infrastructure.Shared;
using Astrum.Infrastructure.Shared.Result.AspNetCore;
using Astrum.Logging.Entities;
using Astrum.Logging.Services;
using Astrum.SharedLib.Common.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Astrum.CodeRev.Backoffice.UserService.Controllers;

[Area("CodeRev")]
[Route("[controller]")]
public class TasksController : ApiBaseController
{
    private const string Solution = "solution";

    private readonly ITaskService _taskService;
    private readonly ILogHttpService _logger;

    public TasksController(ITaskService taskService, ILogHttpService logger)
    {
        this._taskService = taskService;
        _logger = logger;
    }

    // [Authorize(Roles = "Interviewer,HrManager,Admin")]
    [HttpGet]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result<List<TestTaskDto>>))]
    [ProducesResponseType(typeof(List<TestTaskDto>), StatusCodes.Status200OK)]
    public async Task<Result<List<TestTaskDto>>> GetTasks([FromQuery] int offset, [FromQuery] int limit)
    {
        var result = await _taskService.GetAllTasks(offset, limit);
        _logger.Log("", result, HttpContext, "Получение списка задач",
            TypeRequest.GET, ModuleAstrum.CodeRev);
        return result;
    }

    //[Authorize(Roles = "Interviewer,HrManager,Admin")]
    [HttpPost]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result<TaskCreationResponse>))]
    [ProducesResponseType(typeof(TaskCreationResponse), StatusCodes.Status200OK)]
    public async Task<Result<TaskCreationResponse>> PostTask([Required] [FromBody] TaskCreationDto taskCreation)
    {
        var newTask = await _taskService.Create(taskCreation);
        var result = Result<TaskCreationResponse>.Success(new TaskCreationResponse
            { TaskId = newTask.Data });
        _logger.Log(taskCreation, result, HttpContext, "Создание задачи",
            TypeRequest.POST, ModuleAstrum.CodeRev);
        return result;
    }

    //[Authorize(Roles = "Interviewer,HrManager,Admin")]
    [HttpPost("{id}")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    public async Task<Result<bool>> PostTaskTestsCode([Required] [FromRoute(Name = "id")] Guid taskId,
        [Required] [FromBody] TestsCodeDto testsCodeDto)
    {
        var result = await _taskService.TryChangeTaskTestsCode(taskId, testsCodeDto.TestsCode);
        _logger.Log(testsCodeDto, result, HttpContext, "Создание тестового кода для задачи",
            TypeRequest.POST, ModuleAstrum.CodeRev);
        return result;
    }

    //[Authorize(Roles = "Interviewer,HrManager,Admin")]
    [HttpGet($"{Solution}")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result<TaskSolutionInfo>))]
    [ProducesResponseType(typeof(TaskSolutionInfo), StatusCodes.Status200OK)]
    public async Task<Result<TaskSolutionInfo>> GetTaskSolutionInfo(
        [Required] [FromQuery(Name = "id")] string taskSolutionId)
    {
        var token = Request.Headers.Authorization.First().Split(" ")?.LastOrDefault();
        var result = await _taskService.GetTaskSolutionInfo(token, taskSolutionId);
        _logger.Log(taskSolutionId, result, HttpContext, "Получение информации о задании",
            TypeRequest.GET, ModuleAstrum.CodeRev);
        return result;
    }

    //[Authorize(Roles = "Interviewer,HrManager,Admin")]
    [HttpPut($"{Solution}/grade")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    public async Task<Result> PutTaskSolutionGrade([Required] [FromQuery(Name = "id")] string taskSolutionId,
        [Required] [FromQuery(Name = "grade")] int grade)
    {
        if (!Enum.IsDefined(typeof(Grade), grade))
            return Result.Error($"{nameof(grade)} is invalid");
        var result = await _taskService.TryPutTaskSolutionGrade(taskSolutionId, (Grade)grade);
        _logger.Log(grade, result, HttpContext, "Оценка задачи в интервью",
            TypeRequest.PUT, ModuleAstrum.CodeRev);
        return result;
    }

    [HttpPost($"{Solution}/reduce-attempt")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result<int>))]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    public async Task<Result<int>> ReduceTaskSolutionAttempt(
        [Required] [FromQuery(Name = "id")] string taskSolutionId)
    {
        var result = await _taskService.TryReduceTaskSolutionAttempt(taskSolutionId);
        _logger.Log(taskSolutionId, result, HttpContext, "Уменьшение количества попыток на решение задачи",
            TypeRequest.POST, ModuleAstrum.CodeRev);
        return result;
    }
}