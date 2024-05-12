using System.Security.Claims;
using Astrum.Infrastructure.Shared.Extensions;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Astrum.Infrastructure.Shared;

[ApiController]
[Produces("application/json")]
// [ProducesErrorResponseType(typeof(Astrum.Core.Common.Results.Result))]
[ProducesResponseType(typeof(UnprocessableEntityResult), StatusCodes.Status422UnprocessableEntity)]
[ProducesResponseType(typeof(ForbidResult), StatusCodes.Status403Forbidden)]
[ProducesResponseType(typeof(BadRequestObjectResult), StatusCodes.Status400BadRequest)]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
[Authorize(AuthenticationSchemes = "Bearer")]

public abstract class ApiBaseController : Controller
{
    private ILogger? _logger;
    private IMapper? _mapper;
    private ISender? _mediator;

    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
    protected IMapper Mapper => _mapper ??= HttpContext.RequestServices.GetRequiredService<IMapper>();

    protected ILogger Logger => _logger ??= HttpContext.RequestServices.GetRequiredService<ILoggerFactory>()
        .CreateLogger(GetType());


#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
    protected Claim? GetUserClaim()
    {
        return User.GetUserIdClaim();
    }
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.

    // protected ActionResult StatusCodeResponse(IResult response)
    // {
    //     if (response.StatusCode == StatusCodes.Status204NoContent)
    //         return NoContent();
    //     return StatusCode(response.StatusCode, response);
    // }

    protected ActionResult AuthError(string message = null)
    {
        return BadRequest(message ?? "Bad access token format");
    }

    // protected ActionResult BadRequest(string error)
    // {
    //     var response = ResultHelper.CreateErrorBadRequest(error);
    //     return response;
    // }
    //
    // protected ActionResult InternalError(string error)
    // {
    //     var response = Result.Error(error);
    //     return response;
    // }
}