using Astrum.Account.Features.Account.AccountDetails;
//using Astrum.Account.Features.Account.UserEdit.Commands;
using Astrum.Infrastructure.Shared;
using Astrum.Infrastructure.Shared.Result.AspNetCore;
using Astrum.Logging.Services;
using Astrum.SharedLib.Common.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Astrum.Account.Controllers;

[Area("Account")]
[Route("[area]/[controller]")]
public class ProfileController : ApiBaseController
{
    private readonly ILogHttpService _logger;
    public ProfileController(ILogHttpService logger)
    {
        _logger = logger;
    }
    [Obsolete]
    [HttpGet("{id:guid}")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(UserDetailsResultData), StatusCodes.Status200OK)]
    public async Task<Result<UserDetailsResultData>> Details([FromRoute] Guid? id, CancellationToken cancellationToken)
    {
        var query = new AccountDetailsQuery(id);
        var response = await Mediator.Send(query, cancellationToken);
        if (!response.IsSuccess)
        {
            _logger.Log(id, response, HttpContext, "Получены детали пользователя", Logging.Entities.TypeRequest.GET, Logging.Entities.ModuleAstrum.Account);
        }
        return response;
    }

    // TODO сделать подобъект который будет содержать body, желательно разграничить requestObject и command
    //[Obsolete]
    //[HttpPut("{id:guid}")]
    //[TranslateResultToActionResult]
    //[ProducesDefaultResponseType(typeof(Result))]
    //[ProducesResponseType(typeof(EditUserResponse), StatusCodes.Status200OK)]
    //public async Task<Result<EditUserResponse>> Edit([FromRoute] Guid id, [FromBody] UserEditCommand command,
    //    CancellationToken cancellationToken)
    //{
    //    command.Id = id;
    //    var response = await Mediator.Send(command, cancellationToken);
    //    return response;
    //}
}