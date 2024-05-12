using Astrum.Identity.Contracts;
using Astrum.SharedLib.Common.CQS.Implementations;
using Astrum.SharedLib.Common.Results;

namespace Astrum.Identity.Features.Commands;

public sealed class ActivateUserCommand : CommandResult
{
    public ActivateUserCommand(string Username)
    {
        this.Username = Username;
    }

    public string Username { get; init; }

    public void Deconstruct(out string Username)
    {
        Username = this.Username;
    }
}

/// <summary>
///     Activate User Command Handler
/// </summary>
public sealed class ActivateUserCommandHandler : CommandResultHandler<ActivateUserCommand>
{
    private readonly IApplicationUserService _applicationUserService;

    public ActivateUserCommandHandler(IApplicationUserService applicationUserService)
    {
        _applicationUserService = applicationUserService;
    }

    public override async Task<Result> Handle(ActivateUserCommand command, CancellationToken cancellationToken)
    {
        return await _applicationUserService.ActivateUser(command.Username);
    }
}