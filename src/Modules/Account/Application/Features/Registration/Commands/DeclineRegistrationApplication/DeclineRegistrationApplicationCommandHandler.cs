using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.Account.Enums;
using Astrum.Account.Features.Registration.Commands.RegistrationApplicationUpdateStatus;
using Astrum.Account.Repositories;
using Astrum.Account.Specifications.RegistrationApplication;
using Astrum.SharedLib.Common.CQS.Implementations;
using Astrum.SharedLib.Common.Results;

namespace Astrum.Account.Application.Features.Registration.Commands.DeclineRegistrationApplication
{
    public sealed class DeclineRegistrationApplicationCommandHandler : CommandResultHandler<DeclineRegistrationApplicationCommand>
    {
        private readonly IRegistrationApplicationRepository _applicationRepository;

        public DeclineRegistrationApplicationCommandHandler(IRegistrationApplicationRepository applicationRepository)
        {
            _applicationRepository = applicationRepository;
        }

        public override async Task<Result> Handle(
            DeclineRegistrationApplicationCommand command, CancellationToken cancellationToken = default)
        {
            var getRegistrationApplicationByIdSpec = new GetRegistrationApplicationByIdSpec(command.ApplicationId);
            var application =
                await _applicationRepository.FirstOrDefaultAsync(getRegistrationApplicationByIdSpec, cancellationToken);
            if (application == null)
                throw new Exception("Application not found");

            // TODO make it possible and return result with warnings
            if (application.Status == ApplicationStatus.Rejected)
                throw new ValidationException("This registration application has already been rejected");

            // TODO make it possible and return result with warnings
            if (application.Status == ApplicationStatus.Accepted)
                throw new ValidationException("Registration application has been approved");

            application.Status = ApplicationStatus.Rejected;
            await _applicationRepository.UnitOfWork.SaveChangesAsync();
            return Result.Success();
        }
    }
}
