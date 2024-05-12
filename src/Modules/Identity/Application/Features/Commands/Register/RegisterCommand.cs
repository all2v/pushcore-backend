using Astrum.Identity.Models;
using Astrum.Identity.Repositories;
using Astrum.Identity.Specifications;
using Astrum.IdentityServer.Domain.Events;
using Astrum.IdentityServer.Domain.ViewModels;
using Astrum.SharedLib.Application.Contracts.Infrastructure.Shared;
using Astrum.SharedLib.Common.CQS.Implementations;
using Astrum.SharedLib.Common.Results;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Astrum.Identity.Features.Commands.Register;

/// <summary>
///     Represent register command
/// </summary>
public class RegisterCommand : Command<Result<ApplicationUser>> 
{
    /// <summary>
    ///     Username field
    /// </summary>
    public string Username { get; init; }

    /// <summary>
    ///     Name field
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    ///     Surname field
    /// </summary>
    public string Surname { get; init; }

    /// <summary>
    ///     Patronymic field
    /// </summary>
    public string Patronymic { get; init; }

    /// <summary>
    ///     Email field
    /// </summary>
    public string Email { get; init; }

    /// <summary>
    ///     User password field
    /// </summary>
    public string Password { get; init; }

    /// <summary>
    ///     User confirm password field
    /// </summary>
    public string ConfirmPassword { get; init; }
}

public sealed class RegisterCommandHandler : CommandHandler<RegisterCommand, Result<ApplicationUser>>
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly ILocalizationService _localizer;
    private readonly ILogger<RegisterCommandHandler> _logger;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly IApplicationUserRepository _userRepository;
    private readonly SignInManager<ApplicationUser> _signInManager;

    /// <summary>
    /// Represent register command handler
    /// </summary>
    /// <param name="signInManager"></param>
    /// <param name="loggerFactory"></param>
    /// <param name="contextAccessor"></param>
    /// <param name="mapper"></param>
    /// <param name="mediator"></param>
    public RegisterCommandHandler(SignInManager<ApplicationUser> signInManager,
        ILoggerFactory loggerFactory,
        IHttpContextAccessor contextAccessor,
        IMapper mapper,
        IMediator mediator,
        IApplicationUserRepository userRepository)
    {
        _signInManager = signInManager;
        _contextAccessor = contextAccessor;
        _mapper = mapper;
        _mediator = mediator;
        _logger = loggerFactory.CreateLogger<RegisterCommandHandler>();
        _userRepository = userRepository;
    }

    //TODO: fix this shit!
    public override async Task<Result<ApplicationUser>> Handle(RegisterCommand request, CancellationToken cancellationToken = default)
    {
        var createUserCommand = _mapper.Map<CreateUserCommand>(request);
        var result = await _mediator.Send(createUserCommand, cancellationToken);

        if (result.IsSuccess)
        {
            var returnUrl = _contextAccessor.HttpContext?.Request.Query["ReturnUrl"];
            var specification = new GetUserByUsernameSpec(request.Username);
            var user = await _userRepository.FirstOrDefaultAsync(specification);
            if (user == null)
                return Result.NotFound("Пользователь не найден.");

            var mappedUser = new UserViewModel
            {
                Email = user.Email,
                Username = user.UserName,
                FirstName = request.Name,
                LastName = request.Surname,
                Patronymic = request.Patronymic,
                Id = user.Id
            };
            await _mediator.Publish(new ApplicationUserCreatedEvent(user.Id.ToString()!, 1, mappedUser, null, null, "Спасибо за регистрацию на корпоративном портале Astrum!"),
                cancellationToken);
            return Result.Success(user);
        }

        // _notificationService.ErrorNotification(result.MessageWithErrors);
        // ModelState.AddModelError("", "User creation failed.");

        return Result.Error("Невозможно создать пользователя.");
    }
}