using Astrum.Identity.Contracts;
using Astrum.Identity.Models;
using Astrum.SharedLib.Common.CQS.Implementations;
using Astrum.SharedLib.Common.Results;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Astrum.Identity.Features.Commands.ChangePassword;

/// <summary>
/// Change Password command handler
/// </summary>
public sealed class ChangePasswordCommandHandler : CommandResultHandler<ChangePasswordCommand>
{
    private readonly IMediator _mediator;
    private readonly IAuthenticatedUserService _userService;
    private readonly UserManager<ApplicationUser> _userManager;

    /// <summary>
    /// ChangePasswordCommandHandler initializer
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="userService"></param>
    public ChangePasswordCommandHandler(
        IMediator mediator,
        IAuthenticatedUserService userService,
        UserManager<ApplicationUser> userManager
    )
    {
        _mediator = mediator;
        _userService = userService;
        _userManager = userManager;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public override async Task<Result> Handle(ChangePasswordCommand request,
        CancellationToken cancellationToken = default)
    {
        var applicationUser = await _userManager.FindByIdAsync(_userService.UserId.ToString());
        if (applicationUser == null)
            return Result.Error("Пользователь не найден.");

        var passwordHasher = new PasswordHasher<ApplicationUser>();
        var hashResult = passwordHasher.VerifyHashedPassword(applicationUser, applicationUser.PasswordHash, request.Password);
        if (hashResult != PasswordVerificationResult.Success)
            return Result.Invalid(new List<ValidationError>() { new ValidationError { ErrorCode = "401", ErrorMessage = "Некорректный текущий пароль", Identifier = "0" } });

        var changePasswordCommand = new ChangeUserPasswordCommand(request.NewPassword, applicationUser.UserName);
        var result = await _mediator.Send(changePasswordCommand, cancellationToken);

        if (!result.IsSuccess)
            // _notificationService.ErrorNotification(result.Message);
            return result;

        // _notificationService.SuccessNotification(_localizer[ResourceKeys.Notifications_PasswordChange_Success]);
        return result;
    }
}