using Astrum.Account.Aggregates;
using Astrum.Account.Features.Achievement;
using Astrum.Account.Features.Profile;
using Astrum.Account.Features.Profile.Commands;
using Astrum.Account.Repositories;
using Astrum.Account.Specifications.UserProfile;
using Astrum.Identity.Contracts;
using Astrum.Identity.Models;
using Astrum.Identity.Repositories;
using Astrum.Identity.Specifications;
using Astrum.News.Aggregates;
using Astrum.News.Services;
using Astrum.Projects.Services;
using Astrum.Projects.ViewModels.Views;
using Astrum.Storage.Services;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Net.WebSockets;
using Astrum.Account.Application.Services;
using Astrum.IdentityServer.Domain.ViewModels;
using MediatR;
using Astrum.News.ViewModels;
using Astrum.News.Repositories;
using Astrum.News.Specifications;
using Astrum.Identity.Managers;
using Astrum.Account.Application.ViewModels;
using Astrum.Account.Enums;
using Astrum.Market.Application.ViewModels;
using Astrum.SharedLib.Persistence.Helpers;
using Astrum.News.DomainServices.ViewModels;
using Astrum.Storage.Repositories;
using Astrum.SharedLib.Common.Results;
using NetBox.Extensions;
using Astrum.SharedLib.Domain.Enums;
using Astrum.Market.Domain.Enums;
using Astrum.Account.Application.Repositories;
using Astrum.Account.Domain.Specifications;

namespace Astrum.Account.Services;

public class UserProfileService : IUserProfileService
{
    private readonly IAchievementService _achievementService;
    private readonly IContactsService _contactsService;
    private readonly IMapper _mapper;
    private readonly INewsRepository _newsRepository;
    private readonly IUserProfileRepository _profileRepository;
    private readonly IProjectService _projectsService;
    private readonly ISocialNetworksService _socialNetworksService;
    private readonly IApplicationUserRepository _userRepository;
    private readonly ITimelineService _timelineService;
    private readonly IFileStorage _fileStorage;
    private readonly IApplicationUserService _userService;
    private readonly IMediator _mediator;
    private readonly ApplicationUserManager _userManager;
    private readonly IFileRepository _fileRepository;
    private readonly ITransactionRepository _transactionRepository;

    public UserProfileService(IAchievementService achievementService, IApplicationUserRepository userRepository,
        IContactsService contactsService, ISocialNetworksService socialNetworksService, IMediator mediator,
        IUserProfileRepository profileRepository, IProjectService projectService, ITimelineService timelineService,
        INewsRepository newsRepository, IApplicationUserService userService, IFileStorage fileStorage, IMapper mapper,
        ApplicationUserManager userManager, IFileRepository fileRepository, ITransactionRepository transactionRepository)
    {
        _achievementService = achievementService;
        _userRepository = userRepository;
        _contactsService = contactsService;
        _socialNetworksService = socialNetworksService;
        _profileRepository = profileRepository;
        _projectsService = projectService;
        _timelineService = timelineService;
        _newsRepository = newsRepository;
        _userService = userService;
        _fileStorage = fileStorage;
        _mapper = mapper;
        _mediator = mediator;
        _userManager = userManager;
        _fileRepository = fileRepository;
        _transactionRepository = transactionRepository;
    }

    #region IUserProfileService Members

    public async Task CreateUserProfileAsync(UserProfile profile)
    {
        var specification = new GetUserProfileByUserIdSpec(profile.UserId);
        if (await _profileRepository.AnyAsync(specification))
            throw new Exception("Profile for this user already exists");
        var timelines = await _timelineService.CreateAsync(profile.Id);
        profile.Timelines = timelines;
        profile.ActiveTimeline = timelines.FirstOrDefault(x => x.TimelineType == TimelineType.Available).Id;
        await _profileRepository.AddAsync(profile);
        await _profileRepository.UnitOfWork.SaveChangesAsync();
    }

    public async Task<Result<UserProfileResponse>> GetUserProfileAsync(Guid userId)
    {
        var specification = new GetUserProfileByUserIdSpec(userId);
        var profile = await _profileRepository.FirstOrDefaultAsync(specification);
        if (profile == null)
            return Result.NotFound("Профиль пользователя не найден.");

        var response = _mapper.Map<UserProfileResponse>(profile);
        if (profile.AvatarImageId.HasValue)
            response.AvatarUrl = await _fileStorage.GetFileUrl(profile.AvatarImageId.Value);
        return Result.Success(response);
    }

    public async Task<Result<UserProfileResponse>> GetUserProfileByUsernameAsync(string username)
    {
        var user = (await GetUserByUsername(username)).Data;
        if (user == null)
            return Result.NotFound("Пользователь не найден.");

        var specification = new GetUserProfileByUserIdSpec(user.Id);
        var profile = await _profileRepository.FirstOrDefaultAsync(specification);
        if (profile == null)
            return Result.NotFound("Профиль пользователя не найден.");

        var response = _mapper.Map<UserProfileResponse>(profile);
        response.UserName = user.UserName;
        if (profile.AvatarImageId.HasValue)
            response.AvatarUrl = await _fileStorage.GetFileUrl(profile.AvatarImageId.Value);
        if (profile.CoverImageId.HasValue)
            response.CoverUrl = await _fileStorage.GetFileUrl(profile.CoverImageId.Value);


        response.Achievements = _mapper.Map<List<AchievementResponse>>(profile.Achievements.Take(4));
        foreach(var achievement in response.Achievements)
        {
            if (achievement.IconId.HasValue)
                achievement.IconUrl = await _fileStorage.GetFileUrl(achievement.IconId.Value);
        }

        // TODO use specification with take in query 
        var projectResponse = await _projectsService.GetMemberShortInfo(user.Id);
        response.Projects = projectResponse.IsSuccess
            ? projectResponse.Data.TakeLast(3).ToList()
            : new List<MemberShortView>();

        var contacts = new ContactsResponse
        {
            Email = user.Email,
            PhoneNumber=  user.PhoneNumber
        };
        response.Contacts = contacts;

        return Result.Success(response);
    }

    public async Task<Result<EditUserProfileResponse>> GetEditInfoByUsernameAsync(string username)
    {
        var user = (await GetUserByUsername(username)).Data;

        var specification = new GetUserProfileByUserIdSpec(user.Id);
        var profile = await _profileRepository.FirstOrDefaultAsync(specification);
        if (profile == null)
            return Result.NotFound("Профиль пользователя не найден.");
        var response = _mapper.Map<EditUserProfileResponse>(profile);
        if(profile.AvatarImageId.HasValue)
            response.AvatarUrl = await _fileStorage.GetFileUrl(profile.AvatarImageId.Value);
        response.Username = user.UserName;
        response.Contacts = new ContactsResponse { Email = user.Email, PhoneNumber = "" }; //TODO: fix phone number - add it to profile

        return Result.Success(response);
    }

    //public async Task<EditUserProfileResponse> EditUserProfileAsync(UserEditCommand1 command)

    public async Task<Result<EditUserProfileResponse>> EditUserProfileAsync(EditUserProfileCommand command)
    {
        var user = (await GetUserByUsername(command.Username)).Data;

        var specification = new GetUserProfileByUserIdSpec(user.Id);
        var profile = await _profileRepository.FirstOrDefaultAsync(specification);
        if (profile == null)
            return Result.NotFound("Профиль пользователя не найден.");

        profile.Address = command.Address ?? profile.Address;
        profile.Competencies = command.Competencies ?? profile.Competencies;
        
        _mapper.Map(command.ActiveTimeline, profile.ActiveTimeline);
        profile.Timelines = await _timelineService.UpdateAsync(profile, command.Timelines);

        user.UserName = command.NewUsername ?? user.UserName;
        
        await _contactsService.UpdateAsync(user, command.Contacts);
        // user.Email = command.Contacts?.Email ?? user.Email;
        // var userEditRequest = _mapper.Map<UserEditCommand>(user);
        // await _mediator.Send(userEditRequest);
        var socialNetworks = await _socialNetworksService.UpdateAsync(profile, command.SocialNetworks);

        profile.SocialNetworks = socialNetworks;
        try
        {
            await _profileRepository.UnitOfWork.SaveChangesAsync();
            await _userRepository.UnitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message, "Ошибка при обновлении профиля пользователя.");
        }

        var response = _mapper.Map<EditUserProfileResponse>(profile);
        response.Username = user.UserName;
        response.Contacts = new ContactsResponse { Email = user.Email, PhoneNumber = "" }; //TODO: fix phone number - add it to profile

        return Result.Success(response);
    }

    public async Task<Result<UserProfileSummary>> GetUserProfileSummaryAsync(Guid userId)
    {
        var user = (await _userService.GetUserAsync(userId)).Data;
        if (user == null)
            return Result.NotFound("Пользователь не найден.");

        var specification = new GetUserProfileByUserIdSpec(userId);
        var profile = await _profileRepository.FirstOrDefaultAsync(specification);
        if (profile == null)
            return Result.NotFound("Профиль пользователя не найден.");
        var summary = _mapper.Map<UserProfileSummary>(profile);
        summary.UserId = userId;
        summary.Username = user.UserName;
        summary.Email = user.Email;
        summary.PrimaryPhone = user.PhoneNumber;
        summary.SecondaryPhone = user.SecondaryPhoneNumber;
        summary.IsActive = user.IsActive;
        var roles = await _userManager.GetRolesAsync(user);
        summary.Roles = RolesHelper.MapToEnumRoles(roles);

        //summary.Role = userData.Roles.FirstOrDefault()?.Role.Name;

        if (profile.AvatarImageId.HasValue)
            summary.AvatarUrl = await _fileStorage.GetFileUrl(profile.AvatarImageId.Value);

        return Result.Success(summary);
    }

    //TODO: что это за хуйня, почему мы отправляем по каждому пользователю по три запроса в базу
    public async Task<Result<List<UserProfileSummary>>> GetUsersProfilesSummariesAsync(IEnumerable<Guid> usersIds)
    {
        var summaries = new List<UserProfileSummary>();
        foreach (var userId in usersIds)
        {
            var summaryResult = await GetUserProfileSummaryAsync(userId);
            if (summaryResult.IsSuccess)
                summaries.Add(summaryResult.Data);
        }

        return Result.Success(summaries);
    }

    public async Task<Result<UserProfileSummary>> GetUserProfileSummaryAsync(string username)
    {
        var appUserSpec = new GetUserByUsernameSpec(username);
        var user = await _userRepository.FirstOrDefaultAsync(appUserSpec);
        if (user == null)
            return Result.NotFound("Пользователь не найден.");

        var specification = new GetUserProfileByUserIdSpec(user.Id);
        var profile = await _profileRepository.FirstOrDefaultAsync(specification);
        if (profile == null)
            return Result.NotFound("Профиль пользователя не найден.");

        var summary = _mapper.Map<UserProfileSummary>(profile);
        summary.UserId = user.Id;
        summary.Username = user.UserName;
        //summary.Role = userData.Roles.FirstOrDefault()?.Role.Name;

        if (profile.AvatarImageId.HasValue)
            summary.AvatarUrl = await _fileStorage.GetFileUrl(profile.AvatarImageId.Value);

        return Result.Success(summary);
    }

    public async Task<Result<List<UserProfileSummary>>> GetUsersProfilesSummariesAsync(IEnumerable<string> usernames)
    {
        var summaries = new List<UserProfileSummary>();
        foreach (var username in usernames)
        {
            var summary = await GetUserProfileSummaryAsync(username);
            if (summary != null)
                summaries.Add(summary);
        }

        return Result.Success(summaries);
    }

    public async Task<Result<List<UserProfileSummary>>> GetAllUsersProfilesSummariesAsync(UsersFilter? filter = null)
    {
        //var userIds = await _userRepository.Items.Select(user => user.Id).ToListAsync();
        //return await GetUsersProfilesSummariesAsync(userIds);

        var users = (await _userService.GetAllUsersAsync()).Data;

        if (filter?.Email != null)
            users = users.Where(x => x.Email.ToLower().Contains(filter.Email.ToLower())).ToList();
        if (filter?.Login != null)
            users = users.Where(x => x.UserName.ToLower().Contains(filter.Login.ToLower())).ToList();

        var spec = new GetUserProfileByFilterSpec(filter?.PositionId, filter?.Name);
        var profiles = await _profileRepository.ListAsync(spec);
        var summaries = _mapper.Map<List<UserProfileSummary>>(profiles);
        var transactions = await _transactionRepository.ListAsync();
        foreach (var summary in summaries)
        {
            var user = users.FirstOrDefault(x => x.Id == summary.UserId);
            if (user == null)
                continue;

            var profile = profiles.FirstOrDefault(x => x.UserId == summary.UserId);
            summary.Username = user.UserName;
            summary.Email = user.Email;
            summary.PrimaryPhone = user.PhoneNumber;
            summary.SecondaryPhone = user.SecondaryPhoneNumber;
            summary.IsActive = user.IsActive;
            summary.Money = transactions.Where(x => x.UserId == summary.UserId).Sum(x => x.Sum);
            var roles = await _userManager.GetRolesAsync(user);
            summary.Roles = RolesHelper.MapToEnumRoles(roles);

            if (profile.AvatarImageId.HasValue)
                summary.AvatarUrl = await _fileStorage.GetFileUrl(profile.AvatarImageId.Value);
        }

        if(filter?.Role != null)
            summaries = summaries.Where(x => x.Roles?.ToList().Contains((RolesEnum)filter.Role) ?? false).ToList();

        summaries = summaries.Where(x => !string.IsNullOrEmpty(x.Username)).ToList();
        return Result.Success(summaries);
    }

    public async Task<Result<ApplicationUser>> GetUserByUsername(string username)
    {
        var specification = new GetUserByUsernameSpec(username);
        var user = await _userRepository.FirstOrDefaultAsync(specification);
        // var user = await _userService.GetUserByUsername(username);
        if (user == null)
            return Result.NotFound($"User with username {username} not found.");

        return user;
    }

    public async Task ResetTimelines()
    {
        await _timelineService.DeleteAllAsync();

        var profiles = await _profileRepository.ListAsync();
        foreach (var profile in profiles)
        {
            profile.Timelines = await _timelineService.CreateAsync(profile.UserId);
            profile.ActiveTimeline = profile.Timelines.FirstOrDefault(x => x.TimelineType == TimelineType.Available).Id;
        }

        await _profileRepository.UnitOfWork.SaveChangesAsync();
    }

    #endregion

    // private async Task<UserViewModel> GetUserVMByUsername(string username)
    // {
    //     var specification = new GetUserByUsernameSpec(username);
    //     var user = await _userRepository.FirstOrDefaultAsync(specification);
    //     // var user = await _userService.GetUserByUsername(username);
    //     if (user == null)
    //         throw new Exception("User not found");
    //
    //     return _mapper.Map<UserViewModel>(user);
    // }
}
