using System.ComponentModel.DataAnnotations;
using Astrum.CodeRev.Application.UserService.Services;
using Astrum.CodeRev.Application.UserService.Services.Interviews;
using Astrum.CodeRev.Application.UserService.ViewModel.DTO.Interviews;
using Astrum.CodeRev.Application.UserService.ViewModel.DTO.Review;
using Astrum.CodeRev.Domain.Aggregates.Draft;
using Astrum.CodeRev.Domain.Aggregates.Enums;
using Astrum.Infrastructure.Shared;
using Astrum.Infrastructure.Shared.Result.AspNetCore;
using Astrum.Logging.Entities;
using Astrum.Logging.Services;
using Astrum.SharedLib.Application.Helpers;
using Astrum.SharedLib.Common.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Astrum.CodeRev.Backoffice.UserService.Controllers;

[Area("CodeRev")]
[Route("[controller]")]
public class InterviewsController : ApiBaseController
{
    private const string Solution = "solution";

    private readonly IInterviewService _interviewService;
    private readonly IDraftService _draftService;
    private readonly ILogHttpService _logger;


    public InterviewsController(IInterviewService interviewService, IDraftService draftService, ILogHttpService logger)
    {
        this._interviewService = interviewService;
        this._draftService = draftService;
        _logger = logger;
    }

    //[Authorize(Roles = "Interviewer,HrManager,Admin")]
    [HttpGet]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result<List<InterviewDto>>))]
    [ProducesResponseType(typeof(List<InterviewDto>), StatusCodes.Status200OK)]
    public async Task<Result<List<InterviewDto>>> GetInterviews([FromQuery] int offset, int limit)
    {
        return await _interviewService.GetAllInterviews(offset, limit);
    }

    //[Authorize(Roles = "Interviewer,HrManager,Admin")]
    [HttpPost]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result<InterviewCreationResponse>))]
    [ProducesResponseType(typeof(InterviewCreationResponse), StatusCodes.Status200OK)]
    public async Task<Result<InterviewCreationResponse>> CreateInterview(
        [Required] [FromHeader(Name = "Authorization")]
        string authorization,
        [Required] [FromBody] InterviewCreationDto interviewCreation)
    {
        var claims = JwtManager.GetJwtTokenClaims(authorization.Split(" ").LastOrDefault());
        var username = claims.First(claim => claim.Type == JwtRegisteredClaimNames.Sub).Value;
        var result = await _interviewService.CreateInterview(interviewCreation, username);
        _logger.Log(interviewCreation, result, HttpContext, "Создано интервью",
            TypeRequest.POST, ModuleAstrum.CodeRev);
        return result;
    }

    //[Authorize(Roles = "Interviewer,HrManager,Admin")]
    [HttpGet("vacancies")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result<List<string>>))]
    [ProducesResponseType(typeof(List<string>), StatusCodes.Status200OK)]
    public async Task<Result<List<string>>> GetVacancies([FromQuery] int offset, [FromQuery] int limit)
    {
        var result = await _interviewService.GetAllVacancies(offset, limit);
        _logger.Log("", result, HttpContext, "Получены вакансии",
            TypeRequest.GET, ModuleAstrum.CodeRev);

        return result;
    }

    //[Authorize(Roles = "Interviewer,HrManager,Admin")]
    [HttpGet($"{Solution}")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result<InterviewSolutionDto>))]
    [ProducesResponseType(typeof(InterviewSolutionDto), StatusCodes.Status200OK)]
    public async Task<Result<InterviewSolutionDto>> GetInterviewSolutionInfo(
        [Required] [FromQuery(Name = "id")] string interviewSolutionId)
    {
        var token = Request.Headers.Authorization.ToString().Split(" ")?.LastOrDefault();
        var result = await _interviewService.GetInterviewSolutionInfo(token, interviewSolutionId);
        _logger.Log(interviewSolutionId, result, HttpContext, "Получено решение интервью",
            TypeRequest.GET, ModuleAstrum.CodeRev);

        return result;
    }

    //[Authorize(Roles = "Interviewer,HrManager,Admin")]
    [HttpPut($"{Solution}/grade")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public async Task<Result> PutInterviewSolutionGrade(
        [Required] [FromQuery(Name = "id")] string interviewSolutionId,
        [Required] [FromQuery(Name = "grade")] int grade)
    {
        if (!Enum.IsDefined(typeof(Grade), grade))
            return Result.Error($"{nameof(grade)} is invalid");

        var result = await _interviewService.TryPutInterviewSolutionGrade(interviewSolutionId, (Grade)grade);
        _logger.Log(interviewSolutionId, result, HttpContext, "Оценка интервью",
            TypeRequest.PUT, ModuleAstrum.CodeRev);
        return result;
    }

    //[Authorize(Roles = "HrManager,Admin")]
    [HttpPut($"{Solution}/result")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public async Task<Result> PutInterviewSolutionResult(
        [Required] [FromQuery(Name = "id")] string interviewSolutionId,
        [Required] [FromQuery(Name = "result")]
        int interviewResult)
    {
        if (!Enum.IsDefined(typeof(InterviewResult), interviewResult))
            return Result.Error($"{nameof(interviewResult)} is invalid");

        var result = await _interviewService.TryPutInterviewSolutionResult(interviewSolutionId,
            (InterviewResult)interviewResult);
        _logger.Log(interviewSolutionId, result, HttpContext, "Создание итоговой оценки интервью",
            TypeRequest.PUT, ModuleAstrum.CodeRev);
        return result;
    }

    //[Authorize(Roles = "Interviewer,HrManager,Admin")]
    [HttpPut($"{Solution}/comment")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public async Task<Result> PutInterviewSolutionComment(
        [Required] [FromQuery(Name = "id")] string interviewSolutionId,
        [Required] [FromBody] InterviewSolutionComment interviewSolutionComment)
    {
        var result = await _interviewService.TryPutInterviewSolutionComment(interviewSolutionId,
            interviewSolutionComment.ReviewerComment);
        _logger.Log(interviewSolutionId, result, HttpContext, "Написание комментария к интервью",
            TypeRequest.PUT, ModuleAstrum.CodeRev);
        return result;
    }

    //[Authorize(Roles = "Interviewer,HrManager,Admin")]
    [HttpPut($"{Solution}/review")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public async Task<Result> PutInterviewSolutionReview(
        [Required] [FromBody] InterviewSolutionReview interviewSolutionReview)
    {
        if (!Enum.IsDefined(typeof(Grade), interviewSolutionReview.AverageGrade))
            return Result.Error($"{nameof(interviewSolutionReview.AverageGrade)} is invalid");
        if (!Enum.IsDefined(typeof(InterviewResult), interviewSolutionReview.InterviewResult))
            return Result.Error($"{nameof(interviewSolutionReview.InterviewResult)} is invalid");
        var result = await _interviewService.TryPutInterviewSolutionReview(interviewSolutionReview);
        _logger.Log(interviewSolutionReview, result, HttpContext, "Написание отзыва об интервью",
            TypeRequest.PUT, ModuleAstrum.CodeRev);
        return result;
    }

    //[Authorize(Roles = "Interviewer,HrManager,Admin")]
    [HttpGet($"{Solution}/draft")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result<Draft>))]
    [ProducesResponseType(typeof(Draft), StatusCodes.Status200OK)]
    public async Task<Result<Draft>> GetDraft([Required] [FromQuery(Name = "id")] string interviewSolutionId)
    {
        var interviewSolution = await _interviewService.GetInterviewSolution(interviewSolutionId);
        if (!interviewSolution.IsSuccess)
            return Result<Draft>.Error(interviewSolution.MessageWithErrors);

        var result = await _draftService.GetDraft(interviewSolution.Data.ReviewerDraftId);
        _logger.Log(interviewSolutionId, result, HttpContext, "Получены заметки об интервью",
            TypeRequest.GET, ModuleAstrum.CodeRev);
        return result;
    }

    //[Authorize(Roles = "Interviewer,HrManager,Admin")]
    [HttpPost($"{Solution}/draft")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public async Task<Result> CreateDraft([Required] [FromBody] ReviewerDraftDto reviewerDraft)
    {
        var interviewSolution = await
            _interviewService.GetInterviewSolution(reviewerDraft.InterviewSolutionId);
        if (!interviewSolution.IsSuccess)
            return Result.Error("no interview solution with such id, interview or user doesn't exist");
        var result = await _draftService.PutDraft(interviewSolution.Data.ReviewerDraftId, reviewerDraft.Draft);
        _logger.Log(reviewerDraft, result, HttpContext, "Создание заметок об интервью",
            TypeRequest.POST, ModuleAstrum.CodeRev);
        return result;
    }
}