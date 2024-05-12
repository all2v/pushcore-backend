using Astrum.Identity.Contracts;
using Astrum.SharedLib.Common.CQS.Implementations;
using Astrum.SharedLib.Common.Results;

namespace Astrum.Identity.Features.Commands;

public class UpdateUserRolesCommand : CommandResult
{
    public UpdateUserRolesCommand(string Username, List<string> Roles)
    {
        this.Username = Username;
        this.Roles = Roles;
    }

    public string Username { get; init; }
    public List<string> Roles { get; init; }

    public void Deconstruct(out string Username, out List<string> Roles)
    {
        Username = this.Username;
        Roles = this.Roles;
    }
}

/// <summary>
///     Update user roles command handler
/// </summary>
public class UpdateUserRolesCommandHandler : CommandResultHandler<UpdateUserRolesCommand>
{
    private readonly IApplicationUserService _applicationUserService;

    public UpdateUserRolesCommandHandler(IApplicationUserService applicationUserService)
    {
        _applicationUserService = applicationUserService;
    }

    public override async Task<Result> Handle(UpdateUserRolesCommand command,
        CancellationToken cancellationToken = default)
    {
        return await _applicationUserService.UpdateRoles(command.Username, command.Roles);
    }
}