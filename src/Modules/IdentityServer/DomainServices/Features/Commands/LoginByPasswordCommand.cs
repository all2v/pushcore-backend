using Astrum.IdentityServer.DomainServices.ViewModels;
using Astrum.SharedLib.Common.CQS.Implementations;
using Astrum.SharedLib.Common.Results;

namespace Astrum.IdentityServer.DomainServices.Features.Commands;

/// <summary>
///     Login by username and password command
/// </summary>

public sealed class LoginByPasswordCommand : Command<Result<UserTokenResult>>
{
    /// <summary>
    ///     Login command
    /// </summary>
    public LoginByPasswordCommand(string username, string password)
    {
        Username = username;
        Password = password;
    }
        
    public string Username { get; set; }
    public string Password { get; set; }
}