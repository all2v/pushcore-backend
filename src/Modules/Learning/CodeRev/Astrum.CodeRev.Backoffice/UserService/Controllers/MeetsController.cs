using System.ComponentModel.DataAnnotations;
using Astrum.CodeRev.Application.UserService.Services;
using Astrum.CodeRev.Application.UserService.ViewModel.DTO.SyncInterviews;
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
public class MeetsController : ApiBaseController
{
    private readonly IMeetsService _meetsService;
    private readonly ILogHttpService _logger;

    public MeetsController(IMeetsService meetsService, ILogHttpService logger)
    {
        this._meetsService = meetsService;
        _logger = logger;
    }

    //[Authorize(Roles = "Interviewer,HrManager,Admin")]
    [HttpGet]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result<List<MeetInfoDto>>))]
    [ProducesResponseType(typeof(List<MeetInfoDto>), StatusCodes.Status200OK)]
    public async Task<Result<List<MeetInfoDto>>> GetMeets([FromQuery] int offset, [FromQuery] int limit)
    {
        var userId = JwtManager.GetUserIdFromRequest(Request);
        if (userId == null)
            return Result.Error($"Unexpected {nameof(Request.Headers.Authorization)} header value");
        var result = await _meetsService.GetMeets(userId.Value, offset, limit);
        _logger.Log("", result, HttpContext, "Получение списка встреч",
            TypeRequest.GET, ModuleAstrum.CodeRev);
        return result;
    }
}