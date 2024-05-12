using Astrum.SharedLib.Common.CQS.Implementations;

namespace Astrum.Account.Features.Registration.Commands.RegistrationApplicationUpdateStatus;

public class ApproveRegistrationApplicationCommand : CommandResult
{
    public Guid ApplicationId { get; set; }

    public ApproveRegistrationApplicationCommand(Guid applicationId)
    {
        ApplicationId = applicationId;
    }
}