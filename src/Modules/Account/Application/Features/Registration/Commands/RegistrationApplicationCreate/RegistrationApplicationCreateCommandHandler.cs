using Astrum.Account.Services;
using Astrum.SharedLib.Common.Results;

namespace Astrum.Account.Features.Registration.Commands.RegistrationApplicationCreate;

public sealed class
    RegistrationApplicationCreateCommandHandler : BaseCommandRegistrationApplicationHandler<
        RegistrationApplicationCreateCommand>
{
    public RegistrationApplicationCreateCommandHandler(IRegistrationApplicationService registrationApplicationService) :
        base(registrationApplicationService)
    {
    }

    public override async Task<Result<RegistrationApplicationResponse>> Handle(
        RegistrationApplicationCreateCommand command, CancellationToken cancellationToken = default)
    {
        var response = await RegistrationApplicationService.CreateAsync(command);
        return Result.Success(response);
    }
}