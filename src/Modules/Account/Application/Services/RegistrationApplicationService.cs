using System.Net;
using System.Net.Mail;
using Astrum.Account.Aggregates;
using Astrum.Account.Application.Features.Registration.Commands.DeclineRegistrationApplication;
using Astrum.Account.Enums;
using Astrum.Account.Features.Registration;
using Astrum.Account.Features.Registration.Commands.RegistrationApplicationCreate;
using Astrum.Account.Features.Registration.Commands.RegistrationApplicationUpdate;
using Astrum.Account.Features.Registration.Commands.RegistrationApplicationUpdateStatus;
using Astrum.Account.Features.Registration.Queries.GetAllRegistrationApplications;
using Astrum.Account.Repositories;
using Astrum.Account.Specifications.RegistrationApplication;
using Astrum.Identity.Contracts;
using Astrum.Identity.Models;
using Astrum.SharedLib.Application.Contracts.Infrastructure.Shared;
using Astrum.SharedLib.Common.Results;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Astrum.Account.Services;

public class RegistrationApplicationService : IRegistrationApplicationService
{
    private readonly IRegistrationApplicationRepository _applicationRepository;
    private readonly IApplicationUserService _applicationUserService;
    private readonly IMapper _mapper;
    private readonly IPasswordGeneratorService _passwordGeneratorService;
    private readonly IUserProfileService _userProfileService;
    private readonly IMediator _mediator;

    public RegistrationApplicationService(IRegistrationApplicationRepository applicationRepository,
        IApplicationUserService applicationUserService,
        IUserProfileService userProfileService,
        IPasswordGeneratorService passwordGeneratorService,
        IMapper mapper, IMediator mediator)
    {
        _applicationRepository = applicationRepository;
        _applicationUserService = applicationUserService;
        _userProfileService = userProfileService;
        _passwordGeneratorService = passwordGeneratorService;
        _mapper = mapper;
        _mediator = mediator;
    }

    #region IRegistrationApplicationService Members

    public async Task<RegistrationApplicationResponse> CreateAsync(RegistrationApplicationCreateCommand command)
    {
        var application = new RegistrationApplication
        {
            Name = command.Name,
            Surname = command.Surname,
            Patronymic = command.Patronymic,
            Username = command.Username,
            PhoneNumber = command.PhoneNumber,
            Email = command.Email,
            Status = ApplicationStatus.Received
        };

        await _applicationRepository.AddAsync(application);
        await _applicationRepository.UnitOfWork.SaveChangesAsync();

        var response = _mapper.Map<RegistrationApplicationResponse>(application);
        return response;
    }

    public async Task<RegistrationApplicationResponse> DeleteAsync(Guid id)
    {
        var getRegistrationApplicationByIdSpec = new GetRegistrationApplicationByIdSpec(id);
        var application = await _applicationRepository.FirstOrDefaultAsync(getRegistrationApplicationByIdSpec);
        if (application == null)
            throw new Exception("Application not found");

        await _applicationRepository.DeleteAsync(application);
        await _applicationRepository.UnitOfWork.SaveChangesAsync();

        var response = _mapper.Map<RegistrationApplicationResponse>(application);
        return response;
    }

    public async Task<List<RegistrationApplicationResponse>> GetAllAsync()
    {
        var query = new GetRegistrationApplicationsListQuery();
        var result = await _mediator.Send(query);
        return result;
    }

    public async Task<RegistrationApplicationResponse> GetAsync(Guid id)
    {
        var getRegistrationApplicationByIdSpec = new GetRegistrationApplicationByIdSpec(id);
        var application = await _applicationRepository.FirstOrDefaultAsync(getRegistrationApplicationByIdSpec);
        if (application == null)
            throw new Exception("Application not found");

        var response = _mapper.Map<RegistrationApplicationResponse>(application);
        return response;
    }

    public async Task<Result> ApproveRegistrationApplicationAsync(Guid applicationId)
    {
        var command = new ApproveRegistrationApplicationCommand(applicationId);
        return await _mediator.Send(command);
    }

    public async Task<Result> DeclineRegistrationApplicationAsync(Guid applicationId)
    {
        var command = new DeclineRegistrationApplicationCommand(applicationId);
        return await _mediator.Send(command);
    }

    public async Task<RegistrationApplicationResponse> UpdateApplicationStatusAsync(
        RegistrationApplicationUpdateStatusCommand command)
    {
        var getRegistrationApplicationByIdSpec = new GetRegistrationApplicationByIdSpec(command.ApplicationId);
        var application = await _applicationRepository.FirstOrDefaultAsync(getRegistrationApplicationByIdSpec);
        if (application == null)
            throw new Exception("Application not found");

        if (application.Status != ApplicationStatus.Received)
            throw new Exception();

        application.Status = command.Status;
        if (application.Status == ApplicationStatus.Accepted)
        {
            var user = new ApplicationUser
            {
                Name = application.Name,
                UserName = application.Username,
                PhoneNumber = application.PhoneNumber,
                Email = application.Email
            };
            var password = _passwordGeneratorService.GenerateRandomPassword();
            await _applicationUserService.CreateUser(user, password, new List<string>(), true);
            var profile = new UserProfile
            {
                UserId = user.Id,
                SocialNetworks = new SocialNetworks(),
                Name = application.Name,
                Surname = application.Surname,
                Patronymic = application.Patronymic
            };
            await _userProfileService.CreateUserProfileAsync(profile);

            using (var client = new SmtpClient())
            {
                var mailAddress = "testmail1221@mail.ru";
                client.Host = "smtp.mail.ru";
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Port = 25;
                client.Credentials = new NetworkCredential(mailAddress, "PZgbWLxvndv4zFACrfY5");
                var message = new MailMessage();
                message.From = new MailAddress(mailAddress);
                message.To.Add(new MailAddress(application.Email));
                message.Body = password;
                await client.SendMailAsync(message);
            }
        }

        await _applicationRepository.UnitOfWork.SaveChangesAsync();

        var response = _mapper.Map<RegistrationApplicationResponse>(application);
        return response;
    }

    public async Task<RegistrationApplicationResponse> UpdateAsync(RegistrationApplicationUpdateCommand command)
    {
        var getRegistrationApplicationByIdSpec = new GetRegistrationApplicationByIdSpec(command.Id);
        var application = await _applicationRepository.FirstOrDefaultAsync(getRegistrationApplicationByIdSpec);
        if (application == null)
            throw new Exception("Application not found");

        application.Name = command.Name ?? application.Name;
        application.Surname = command.Surname ?? application.Surname;
        application.Patronymic = command.Patronymic ?? application.Patronymic;
        application.Username = command.Username ?? application.Username;
        application.PhoneNumber = command.PhoneNumber ?? application.PhoneNumber;
        application.Email = command.Email ?? application.Email;

        await _applicationRepository.UnitOfWork.SaveChangesAsync();

        var response = _mapper.Map<RegistrationApplicationResponse>(application);
        return response;
    }

    #endregion
}