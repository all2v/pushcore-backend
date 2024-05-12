using Astrum.Account.Enums;
using Astrum.SharedLib.Common.CQS.Implementations;
using Microsoft.AspNetCore.Mvc;

namespace Astrum.Account.Features.Registration.Commands.RegistrationApplicationUpdateStatus;

public class RegistrationApplicationUpdateStatusCommand : CommandResult<RegistrationApplicationResponse>
{
    public Guid ApplicationId { get; set; }
    public ApplicationStatus Status { get; set; }
}
