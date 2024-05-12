using Astrum.Account.Services;
using Astrum.SharedLib.Common.CQS.Implementations;
using Astrum.SharedLib.Common.CQS.Interfaces;
using Astrum.SharedLib.Common.Results;

namespace Astrum.Account.Features.Registration;

public abstract class
    BaseCommandRegistrationApplicationHandler<TCommand> : CommandResultHandler<TCommand,
        RegistrationApplicationResponse> where TCommand : class, ICommand<Result<RegistrationApplicationResponse>>
{
    protected readonly IRegistrationApplicationService RegistrationApplicationService;

    public BaseCommandRegistrationApplicationHandler(IRegistrationApplicationService registrationApplicationService)
    {
        RegistrationApplicationService = registrationApplicationService;
    }
}