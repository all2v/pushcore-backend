using System.Threading;
using Astrum.Account.Aggregates;
using Astrum.Account.DomainServices.Features.Commands;
using Astrum.Account.Enums;
using Astrum.Account.Repositories;
using Astrum.Account.Specifications.UserProfile;
using Astrum.IdentityServer.Domain.Events;
using Astrum.SharedLib.Application.Contracts.Infrastructure;
using Astrum.SharedLib.Application.Contracts.Infrastructure.Shared;
using Astrum.SharedLib.Application.Models;
using Astrum.SharedLib.Application.Models.Email;
using Astrum.SharedLib.Domain.EventHandlers;
using AutoMapper;
using MediatR;

namespace Astrum.Account.EventHandlers;

public class ApplicationUserCreatedEventHandler : DomainEventHandler<ApplicationUserCreatedEvent>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IUserProfileRepository _userProfileRepository;
    private readonly ITimelineRepository _timelineRepository;
    private readonly IPasswordGeneratorService _passwordGeneratorService;
    private readonly IEmailService _emailService;
    private readonly IMediator _mediator;

    public ApplicationUserCreatedEventHandler(IAccountRepository accountRepository, 
        IUserProfileRepository userProfileRepository,
        ITimelineRepository timelineRepository, IPasswordGeneratorService passwordGeneratorService,
        IEmailService emailService, IMediator mediator)
    {
        _accountRepository = accountRepository;
        _userProfileRepository = userProfileRepository;
        _timelineRepository = timelineRepository;
        _passwordGeneratorService = passwordGeneratorService;
        _emailService = emailService;
        _mediator = mediator;
    }

    public override async Task Handle(ApplicationUserCreatedEvent @event, CancellationToken cancellationToken)
    {
        //var account = new Aggregates.Account(@event.AggregateId)
        //{
        //    PhoneNumber = @event.PhoneNumber
        //};

        //await _accountRepository.AddAsync(account, cancellationToken);
        //await _accountRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        // пока так, не знаю, почему сюда два раза заходит
        try 
        {   
            if(!Guid.TryParse(@event.AggregateId, out var guid))
                return;
            var spec = new GetUserProfileByUserIdSpec(guid);
            if (await _userProfileRepository.AnyAsync(spec))
                return;

            var bucketId = await CreateBucketAsync(guid);
            var timelines = await CreateTimelinesAsync(guid);
            var profile = await CreateUserProfileAsync(@event, timelines, bucketId);
            await SendEmailWithPassword(@event);
        } 
        catch (Exception ex) 
        {

        }
    }

    private async Task<Guid> CreateBucketAsync(Guid userId)
    {
        return await _mediator.Send(new CreateBucketCommand(userId));
    }

    private async Task<UserProfile> CreateUserProfileAsync(ApplicationUserCreatedEvent @event, 
        List<AccessTimeline> timelines, Guid bucketId)
    {
        var profile = new UserProfile()
        {
            UserId = Guid.Parse(@event.AggregateId),
            SocialNetworks = new SocialNetworks(),
            Name = @event.Application?.Name ?? @event.User.FirstName,
            Surname = @event.Application?.Surname ?? @event.User.LastName ?? "",
            Patronymic = @event.Application?.Patronymic ?? @event.User.Patronymic ?? "",
            BasketId = bucketId
        };
        profile.Timelines = timelines;
        profile.ActiveTimeline = timelines.FirstOrDefault(x => x.TimelineType == TimelineType.Available).Id;
        var response = await _userProfileRepository.AddAsync(profile);
        await _userProfileRepository.UnitOfWork.SaveChangesAsync();
        return response;
    }

    private async Task<List<AccessTimeline>> CreateTimelinesAsync(Guid userId)
    {
        var timelines = new List<AccessTimeline>();
        foreach (TimelineType type in Enum.GetValues(typeof(TimelineType)))
        {
            var timeline = new AccessTimeline
            {
                UserId = userId,
                TimelineType = type,
                Intervals = new List<AccessTimelineInterval>()
            };
            var intervalType = type == TimelineType.Available ? TimelineIntervalType.Available :
                type == TimelineType.BusinessTrip ? TimelineIntervalType.MessageOnly : TimelineIntervalType.Unavailable;
            var interval = new AccessTimelineInterval
            {
                StartTime = new TimeSpan(0, 0, 0),
                EndTime = new TimeSpan(0, 0, 0),
                TimelineId = timeline.Id,
                IntervalType = intervalType
            };
            timeline.Intervals.Add(interval);
            timeline = await _timelineRepository.AddAsync(timeline);
            timelines.Add(timeline);
        }
        
        await _timelineRepository.UnitOfWork.SaveChangesAsync();
        return timelines;
    }

    private async Task<bool> SendEmailWithPassword(ApplicationUserCreatedEvent @event)
    {
        if (@event.Password == null || @event.User == null)
            return false;

        var email = new Email()
        {
            To = @event.User.Email,
            Subject = "Регистрация на портале",
            Body = @event.Password
        };

        return await _emailService.SendEmail(email);
    }
}