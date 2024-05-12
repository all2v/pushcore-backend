using Astrum.Account.Services;
using Astrum.SharedLib.Common.Results;

namespace Astrum.Account.Features.Registration.Commands.RegistrationApplicationUpdateStatus;

public sealed class RegistrationApplicationUpdateStatusCommandHandler :
    BaseCommandRegistrationApplicationHandler<RegistrationApplicationUpdateStatusCommand>
{
    public RegistrationApplicationUpdateStatusCommandHandler(
        IRegistrationApplicationService registrationApplicationService) : base(registrationApplicationService)
    {
    }

    public override async Task<Result<RegistrationApplicationResponse>> Handle(
        RegistrationApplicationUpdateStatusCommand command, CancellationToken cancellationToken = default)
    {
        var response = await RegistrationApplicationService.UpdateApplicationStatusAsync(command);
        return Result.Success(response);
    }
}
