using Astrum.SharedLib.Common.CQS.Implementations;

namespace Astrum.Identity.Features.Commands.ChangePassword;

/// <summary>
///     Represent a change password command type
/// </summary>
public class ChangePasswordCommand : CommandResult 
{
    /// <summary>
    ///     Field for current user password
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    ///     Field for user new password
    /// </summary>
    public string NewPassword { get; set; }

    /// <summary>
    ///     Field for confirm password
    /// </summary>
    //public string ConfirmPassword { get; set; }
}