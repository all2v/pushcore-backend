using Astrum.Account.Aggregates;
using Astrum.Account.Repositories;
using Astrum.Account.Services;
using Astrum.Account.Specifications.RegistrationApplication;
using Astrum.Identity.Features.Commands;
using Astrum.Identity.Repositories;
using Astrum.Identity.Specifications;
using Astrum.IdentityServer.Domain.Events;
using Astrum.IdentityServer.Domain.ViewModels;
using Astrum.SharedLib.Application.Contracts.Infrastructure.Shared;
using Astrum.SharedLib.Domain.EventHandlers;
using Astrum.SharedLib.Domain.Interfaces;
using AutoMapper;
using FluentValidation;
using Keycloak.AuthServices.Sdk.Admin;
using Keycloak.AuthServices.Sdk.Admin.Models;
using MediatR;

namespace Astrum.IdentityServer.DomainServices.EventHandlers;

public class ApplicationStatusChangedEventHandler : DomainEventHandler<ApplicationStatusChangedEvent>
{
    private readonly IRegistrationApplicationRepository _registrationApplicationRepository;
    private readonly IPasswordGeneratorService _passwordGeneratorService;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly IApplicationUserRepository _userRepository;

    public ApplicationStatusChangedEventHandler(IRegistrationApplicationRepository registrationApplicationRepository, IMapper mapper,
        IMediator mediator, IPasswordGeneratorService passwordGeneratorService, IApplicationUserRepository userRepository)
    {
        _registrationApplicationRepository = registrationApplicationRepository;
        _mediator = mediator;
        _passwordGeneratorService = passwordGeneratorService;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public override async Task Handle(ApplicationStatusChangedEvent @event, CancellationToken cancellationToken)
    {
        if (@event.AggregateId == Guid.Empty)
            return;

        var getRegistrationApplicationByIdSpec = new GetRegistrationApplicationByIdSpec(@event.AggregateId);
        var application =
            await _registrationApplicationRepository.FirstOrDefaultAsync(getRegistrationApplicationByIdSpec,
                cancellationToken);
        if (application is null)
            throw new ValidationException("Registration application not found");

        var password = _passwordGeneratorService.GenerateRandomPassword();
        
        if (application.Status == Account.Enums.ApplicationStatus.Accepted)
        {
            var identityUser = new CreateUserCommand()
            {
                Name = $"{application.Surname} {application.Name} {application.Patronymic}",
                Username = application.Username,
                Email = application.Email,
                Password = password
            };

            await _mediator.Send(identityUser);

            // TODO implement it via custom http client. We should get it for user id receiving  
            var specification = new GetUserByUsernameSpec(application.Username);
            var user = await _userRepository.FirstOrDefaultAsync(specification);
            // var user = await _userService.GetUserByUsername(username);
            if (user == null)
                throw new Exception("User not found");

            var mappedUser = new UserViewModel
            { 
                Email = application.Email,
                Username = application.Username,
                FirstName = application.Name,
                LastName = application.Surname, 
                Patronymic = application.Patronymic,
                Id = user.Id
            };

            await _mediator.Publish(new ApplicationUserCreatedEvent(user.Id.ToString()!, 1, mappedUser, application.PhoneNumber, application, password),
                cancellationToken);
        }
    }
}