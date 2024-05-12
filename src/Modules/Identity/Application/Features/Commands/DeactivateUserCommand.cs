using Astrum.Identity.Contracts;
using Astrum.SharedLib.Common.CQS.Implementations;
using Astrum.SharedLib.Common.Results;

namespace Astrum.Identity.Features.Commands;

public class DeactivateUserCommand : CommandResult
{
    public DeactivateUserCommand(string Username)
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
///     Deactivate User Command Handler
/// </summary>
public class DeactivateUserCommandHandler : CommandResultHandler<DeactivateUserCommand>
{
    private readonly IApplicationUserService _applicationUserService;

    public DeactivateUserCommandHandler(IApplicationUserService applicationUserService)
    {
        _applicationUserService = applicationUserService;
    }

    public override async Task<Result> Handle(DeactivateUserCommand command,
        CancellationToken cancellationToken = default)
    {
        return await _applicationUserService.DeactivateUser(command.Username);
    }
}