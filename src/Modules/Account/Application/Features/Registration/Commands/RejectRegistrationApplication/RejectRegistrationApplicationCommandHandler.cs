using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.Account.Enums;
using Astrum.Account.Repositories;
using Astrum.Account.Specifications.RegistrationApplication;
using Astrum.SharedLib.Common.CQS.Implementations;
using Astrum.SharedLib.Common.Results;
using FluentValidation;

namespace Astrum.Account.Application.Features.Registration.Commands.RejectRegistrationApplication
{
    public sealed class RejectRegistrationApplicationCommandHandler : CommandResultHandler<RejectRegistrationApplicationCommand>
    {
        private readonly IRegistrationApplicationRepository _applicationRepository;

        public RejectRegistrationApplicationCommandHandler(IRegistrationApplicationRepository applicationRepository) 
        {
            _applicationRepository = applicationRepository;
        }

        public override async Task<Result> Handle(
            RejectRegistrationApplicationCommand command, CancellationToken cancellationToken = default)
        {
            var getRegistrationApplicationByIdSpec = new GetRegistrationApplicationByIdSpec(command.ApplicationId);
            var application =
                await _applicationRepository.FirstOrDefaultAsync(getRegistrationApplicationByIdSpec, cancellationToken);
            if (application == null)
                throw new Exception("Application not found");

            // TODO make it possible and return result with warnings
            if (application.Status == ApplicationStatus.Rejected)
                throw new ValidationException("Registration application has already been rejected");

            // TODO make it possible and return result with warnings
            if (application.Status == ApplicationStatus.Accepted)
                throw new ValidationException("This registration application has been accepted");

            application.Status = ApplicationStatus.Rejected;
            await _applicationRepository.UnitOfWork.SaveChangesAsync();
            return Result.Success();
        }
    }
}
