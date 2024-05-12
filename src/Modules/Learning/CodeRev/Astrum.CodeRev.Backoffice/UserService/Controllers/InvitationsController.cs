using System.ComponentModel.DataAnnotations;
using Astrum.CodeRev.Application.UserService.Services;
using Astrum.CodeRev.Application.UserService.ViewModel.DTO;
using Astrum.CodeRev.Application.UserService.ViewModel.DTO.Users;
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
public class InvitationsController : ApiBaseController
{
    private readonly IInvitationService _invitationService;
    private readonly ILogHttpService _logger;


    public InvitationsController(IInvitationService invitationService, ILogHttpService logger)
    {
        this._invitationService = invitationService;
        _logger = logger;
    }

    //[Authorize(Roles = "Interviewer,HrManager,Admin")]
    [HttpPost("create")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result<InvitationResponse>))]
    [ProducesResponseType(typeof(InvitationResponse), StatusCodes.Status200OK)]
    public async Task<Result<InvitationResponse>> CreateInvitation([FromBody] InvitationParams invitationParams)
    {
        var userId = JwtManager.GetUserIdFromRequest(Request);
        if (userId == null)
            return Result.Error($"Unexpected {nameof(Request.Headers.Authorization)} header value");

        var result = await _invitationService.Create(invitationParams, userId.Value);
        _logger.Log(invitationParams, result, HttpContext, "Создание приглашения",
            TypeRequest.POST, ModuleAstrum.CodeRev);
        return result;
    }

    [HttpGet("validate")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    public async Task<Result> ValidateInvitationAsync(
        [Required] [FromQuery(Name = "invite")]
        string invitationId)
    {
        var result = await _invitationService.CheckForDeadline(invitationId);
        _logger.Log(invitationId, result, HttpContext, "Проверка приглашения на дедлайн",
            TypeRequest.GET, ModuleAstrum.CodeRev);
        return result;
    }
}