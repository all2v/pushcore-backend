using Astrum.SharedLib.Common.CQS.Implementations;
using Astrum.SharedLib.Common.Results;
using Microsoft.Extensions.Logging;

namespace Astrum.Identity.Features.Commands.Logout;

public class LogoutCommand : CommandResult 
{
}

public sealed class LogoutCommandHandler :CommandResultHandler<LogoutCommand>
{
    private readonly ILogger _logger;
    // private readonly IUserAuthenticationService _userAuthenticationService;

    public LogoutCommandHandler(
            ILoggerFactory loggerFactory)
        // IUserAuthenticationService userAuthenticationService)
    {
        // _userAuthenticationService = userAuthenticationService;
        _logger = loggerFactory.CreateLogger<LogoutCommandHandler>();
    }

    public override async Task<Result> Handle(LogoutCommand request, CancellationToken cancellationToken = default)
    {
        // await _userAuthenticationService.Logout(cancellationToken);
        return Result.Success();
    }
}