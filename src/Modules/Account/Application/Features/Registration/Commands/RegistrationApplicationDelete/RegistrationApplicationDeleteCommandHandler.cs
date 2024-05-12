using Astrum.Account.Services;
using Astrum.SharedLib.Common.Results;

namespace Astrum.Account.Features.Registration.Commands.RegistrationApplicationDelete;

public sealed class
    RegistrationApplicationDeleteCommandHandler : BaseCommandRegistrationApplicationHandler<
        RegistrationApplicationDeleteCommand>
{
    public RegistrationApplicationDeleteCommandHandler(IRegistrationApplicationService registrationApplicationService) :
        base(registrationApplicationService)
    {
    }

    public override async Task<Result<RegistrationApplicationResponse>> Handle(
        RegistrationApplicationDeleteCommand command, CancellationToken cancellationToken = default)
    {
        var response = await RegistrationApplicationService.DeleteAsync(command.Id);
        return Result.Success(response);
    }
}