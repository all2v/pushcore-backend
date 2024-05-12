using System.ComponentModel.DataAnnotations;
using Astrum.CodeRev.Application.UserService.Services;
using Astrum.CodeRev.Application.UserService.ViewModel.DTO.Auth;
using Astrum.Infrastructure.Shared.Result.AspNetCore;
using Astrum.Logging.Entities;
using Astrum.Logging.Services;
using Astrum.SharedLib.Common.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Astrum.CodeRev.Backoffice.UserService.Controllers;

[Area("CodeRev")]
[Route("[controller]")]
public class UsersController : Controller
{
    private readonly IUserService _userService;
    private readonly ILogHttpService _logger;


    public UsersController(IUserService userService, ILogHttpService logger)
    {
        this._userService = userService;
        _logger = logger;
    }

    [HttpPost("accept-invite")] //todo тут получается передается токен нужного пользователя 
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    public async Task<Result<Guid>> Invite([Required] [FromQuery(Name = "invite")] Guid invitationId
        , [Required] [FromBody] UserInvite userInvite)
    {
        var invitedUserToken =
            Request.Headers.Authorization
                .First()?.Split(" ")
                .LastOrDefault();
        var result = await _userService.CreateInterviewSolution(invitedUserToken, userInvite, invitationId);
        _logger.Log(userInvite, result, HttpContext, "Создание комнаты интервью для приглашенного пользователя",
            TypeRequest.POST, ModuleAstrum.CodeRev);
        return result;
    }
}