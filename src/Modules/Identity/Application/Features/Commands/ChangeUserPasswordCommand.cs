using Astrum.Identity.Contracts;
using Astrum.SharedLib.Common.CQS.Implementations;
using Astrum.SharedLib.Common.Results;

namespace Astrum.Identity.Features.Commands;

public class ChangeUserPasswordCommand : CommandResult 
{
    public ChangeUserPasswordCommand(string password, string username)
    {
        Password = password;
        Username = username;
    }

    public string Username { get; set; }
    public string Password { get; set; }
}

/// <summary>
///     Create User Command Handler
/// </summary>
public class ChangeUserPasswordCommandHandler : CommandResultHandler<ChangeUserPasswordCommand>
{
    private readonly IApplicationUserService _applicationUserService;

    public ChangeUserPasswordCommandHandler(IApplicationUserService applicationUserService)
    {
        _applicationUserService = applicationUserService;
    }

    public override async Task<Result> Handle(ChangeUserPasswordCommand command,
        CancellationToken cancellationToken = default)
    {
        return await _applicationUserService.ChangePassword(command.Username, command.Password);
    }
}