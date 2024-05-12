using Astrum.Identity.Application.ViewModels;
using Astrum.Identity.Contracts;
using Astrum.Identity.DTOs;
using Astrum.SharedLib.Common.CQS.Implementations;
using Astrum.SharedLib.Common.Results;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace Astrum.Identity.Features.Commands.Login;

public sealed class LoginCommandHandler : CommandHandler<LoginCommand, Result<UserTokenResult>>
{
    private readonly ILogger _logger;

    private readonly IMapper _mapper;
    private readonly IUserAuthenticationService _userAuthenticationService;

    public LoginCommandHandler(
        ILoggerFactory loggerFactory,
        IMapper mapper,
        IUserAuthenticationService userAuthenticationService
    )
    {
        _mapper = mapper;
        _userAuthenticationService = userAuthenticationService;
        _logger = loggerFactory.CreateLogger<LoginCommand>();
    }

    public override async Task<Result<UserTokenResult>> Handle(LoginCommand request, CancellationToken cancellationToken = default)
    {
        var loginDto = _mapper.Map<LoginRequestDTO>(request);
        var result = await _userAuthenticationService.Login(loginDto, cancellationToken);

        // if (result.Succeeded) _logger.LogWarning(1, "Юзер вошел в систему");
        // return result;
        if (result == null)
            return Result.Error("Непридвиденная ошибка.");
        if (result.Successful)
            return Result.Success(result);
        if (result.ErrorMessage == "Ошибка при входе пользователя.")
            return Result.Error(result.ErrorMessage.Split(":::")[0], result.ErrorMessage.Split(":::")[1]);
        return Result.Invalid(new List<ValidationError> { new ValidationError { ErrorCode = "400", ErrorMessage = result.ErrorMessage, Identifier = "0" } });
    }
}