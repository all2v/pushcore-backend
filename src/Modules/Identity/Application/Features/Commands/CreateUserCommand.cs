using Astrum.Identity.Contracts;
using Astrum.Identity.Models;
using Astrum.SharedLib.Common.CQS.Implementations;
using Astrum.SharedLib.Common.Results;

namespace Astrum.Identity.Features.Commands;

public class CreateUserCommand : CommandResult 
{
    public CreateUserCommand()
    {
    }
    public CreateUserCommand(string username, string name, string email, string password, List<string>? roles)
    {
        Username = username;
        Name = name;
        Email = email;
        Password = password;
        Roles = roles;
    }

    public string Username { get; init; }
    public string Name { get; init; }
    public string Email { get; init; }
    public string Password { get; init; }
    public List<string>? Roles { get; init; }
}

/// <summary>
///     Create User Command Handler
/// </summary>
public class CreateUserCommandHandler : CommandResultHandler<CreateUserCommand>
{
    private readonly IApplicationUserService _applicationUserService;

    public CreateUserCommandHandler(IApplicationUserService applicationUserService)
    {
        _applicationUserService = applicationUserService;
    }

    public override async Task<Result> Handle(CreateUserCommand command, CancellationToken cancellationToken = default)
    {
        var user = new ApplicationUser
        {
            UserName = command.Username, Email = command.Email, Name = command.Name
        };
        return await _applicationUserService.CreateUser(user, command.Password, command.Roles, true);
    }
}