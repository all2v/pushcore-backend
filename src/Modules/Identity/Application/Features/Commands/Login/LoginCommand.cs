using Astrum.Identity.Application.ViewModels;
using Astrum.SharedLib.Common.CQS.Implementations;
using Astrum.SharedLib.Common.Results;

namespace Astrum.Identity.Features.Commands.Login;

/// <summary>
///     Login command
/// </summary>
public sealed class LoginCommand : Command<Result<UserTokenResult>>
{
    /// <summary>
    ///     Login command
    /// </summary>
    /// <param name="Login">Username or email for authentication</param>
    /// <param name="Password">Password for authentication</param>
    public LoginCommand(string Login, string Password/*, string? ReturnUrl, bool RememberMe = false*/)
    {
        this.Login = Login;
        this.Password = Password;
        //this.ReturnUrl = ReturnUrl;
        //this.RememberMe = RememberMe;
    }

    /// <summary>Username or email for authentication</summary>
    public string Login { get; init; }

    /// <summary>Password for authentication</summary>
    public string Password { get; init; }

    ///// <summary> The path where client should be redirected</summary>
    //public string? ReturnUrl { get; init; }

    ///// <summary> Flag indicating whether the sign-in cookie should persist after the browser is closed.</summary>
    //public bool RememberMe { get; init; }

    public void Deconstruct(out string Login, out string Password/*, out string? ReturnUrl, out bool RememberMe*/)
    {
        Login = this.Login;
        Password = this.Password;
        //ReturnUrl = this.ReturnUrl;
        //RememberMe = this.RememberMe;
    }
};