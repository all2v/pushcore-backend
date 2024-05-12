using Astrum.Account.Services;
using Astrum.SharedLib.Common.Results;

namespace Astrum.Account.Features.Registration.Commands.RegistrationApplicationUpdate;

public sealed class
    RegistrationApplicationUpdateCommandHandler : BaseCommandRegistrationApplicationHandler<
        RegistrationApplicationUpdateCommand>
{
    public RegistrationApplicationUpdateCommandHandler(IRegistrationApplicationService registrationApplicationService) :
        base(registrationApplicationService)
    {
    }

    public override async Task<Result<RegistrationApplicationResponse>> Handle(
        RegistrationApplicationUpdateCommand command, CancellationToken cancellationToken = default)
    {
        var response = await RegistrationApplicationService.UpdateAsync(command);
        return Result.Success(response);
    }
}