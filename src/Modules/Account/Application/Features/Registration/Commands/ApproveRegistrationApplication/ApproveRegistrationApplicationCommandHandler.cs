using Astrum.Account.Enums;
using Astrum.Account.Repositories;
using Astrum.Account.Specifications.RegistrationApplication;
using Astrum.SharedLib.Common.CQS.Implementations;
using Astrum.SharedLib.Common.Results;
using FluentValidation;

namespace Astrum.Account.Features.Registration.Commands.RegistrationApplicationUpdateStatus;

public sealed class
    ApproveRegistrationApplicationCommandHandler : CommandResultHandler<ApproveRegistrationApplicationCommand>
{
    private readonly IRegistrationApplicationRepository _applicationRepository;

    public ApproveRegistrationApplicationCommandHandler(IRegistrationApplicationRepository applicationRepository)
    {
        _applicationRepository = applicationRepository;
    }

    public override async Task<Result> Handle(
        ApproveRegistrationApplicationCommand command, CancellationToken cancellationToken = default)
    {
        var getRegistrationApplicationByIdSpec = new GetRegistrationApplicationByIdSpec(command.ApplicationId);
        var application =
            await _applicationRepository.FirstOrDefaultAsync(getRegistrationApplicationByIdSpec, cancellationToken);
        if (application == null)
            throw new Exception("Application not found");
        
        // TODO make it possible and return result with warnings
        if (application.Status == ApplicationStatus.Accepted)
            throw new ValidationException("This registration application has already been approved");

        // TODO make it possible and return result with warnings
        if (application.Status == ApplicationStatus.Rejected)
            throw new ValidationException("Registration application has been rejected");

        application.Status = ApplicationStatus.Accepted;
        await _applicationRepository.UnitOfWork.SaveChangesAsync();
        return Result.Success();
    }
}