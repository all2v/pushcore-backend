using Astrum.Identity.Contracts;
using Astrum.Identity.DTOs;
using Astrum.SharedLib.Common.CQS.Implementations;
using Astrum.SharedLib.Common.Results;

namespace Astrum.Identity.Features.Commands;

public class AddRolesCommand : CommandResult
{
    public AddRolesCommand(string Username, List<string> Roles)
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
///     Add roles to user command handler
/// </summary>
public class AddRolesCommandHandler : CommandResultHandler<AddRolesCommand>
{
    private readonly IApplicationUserService _applicationUserService;

    public AddRolesCommandHandler(IApplicationUserService applicationUserService)
    {
        _applicationUserService = applicationUserService;
    }

    public override async Task<Result> Handle(AddRolesCommand command, CancellationToken cancellationToken = default)
    {
        var request = new RoleAssignmentRequestDto
        (
            command.Username,
            command.Roles
        );

        return await _applicationUserService.AddRoles(request);
    }
}