using Astrum.Account.Repositories;
using Astrum.Account.Services;
using Astrum.Account.Specifications.UserProfile;
using Astrum.Identity.Repositories;
using Astrum.Identity.Specifications;
using Astrum.SharedLib.Common.CQS.Implementations;
using Astrum.SharedLib.Common.Results;
using Astrum.Storage.Services;
using AutoMapper;

namespace Astrum.Account.Features.Profile.Commands;

public class EditUserProfileCommandHandler : CommandResultHandler<EditUserProfileCommand, EditUserProfileResponse>
{
    private readonly IApplicationUserRepository _userRepository;
    private readonly IUserProfileRepository _profileRepository;
    private readonly IMapper _mapper;
    private readonly ITimelineService _timelineService;
    private readonly ISocialNetworksService _socialNetworksService;
    private readonly IFileStorage _fileStorage; 

    public EditUserProfileCommandHandler(IApplicationUserRepository userRepository, IUserProfileRepository profileRepository, 
        IMapper mapper, ITimelineService timelineService, ISocialNetworksService socialNetworksService,
        IFileStorage fileStorage)
    {
        _userRepository = userRepository;
        _profileRepository = profileRepository;
        _mapper = mapper;
        _timelineService = timelineService;
        _socialNetworksService = socialNetworksService;
        _fileStorage = fileStorage;
    }

    public override async Task<Result<EditUserProfileResponse>> Handle(EditUserProfileCommand command,
        CancellationToken cancellationToken = default)
    {
        var userSpec = new GetUserByUsernameSpec(command.Username);
        var user = await _userRepository.FirstOrDefaultAsync(userSpec, cancellationToken);

        if (user == null)
            return Result.NotFound($"Пользователь не найден.");

        var profileSpec = new GetUserProfileByUserIdSpec(user.Id);
        var profile = await _profileRepository.FirstOrDefaultAsync(profileSpec, cancellationToken);

        if (profile == null)
            return Result.NotFound($"Профиль пользователя не найден.");

        profile.RequisiteBank = command.RequisiteBank ?? profile.RequisiteBank;
        profile.RequisiteNumberPhone = command.RequisiteNumberPhone ?? profile.RequisiteNumberPhone;
        profile.Address = command.Address ?? profile.Address;
        profile.Competencies = command.Competencies ?? profile.Competencies;

        if (command.Timelines != null)
            profile.Timelines = await _timelineService.UpdateAsync(profile, command.Timelines);
        if (command.ActiveTimeline != null)
            profile.ActiveTimeline = profile.Timelines
                .FirstOrDefault(x => x.TimelineType == command.ActiveTimeline.TimelineType).Id;
        if (command.SocialNetworks != null)
            profile.SocialNetworks = await _socialNetworksService.UpdateAsync(profile, command.SocialNetworks);

        await _profileRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        user.UserName = command.NewUsername ?? user.UserName;
        user.NormalizedUserName = user.UserName?.ToUpper();
        user.PhoneNumber = command.Contacts?.PhoneNumber ?? user.PhoneNumber;
        user.Email = command.Contacts?.Email ?? user.Email;

        await _userRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        var response = _mapper.Map<EditUserProfileResponse>(profile);
        response.Username = user.UserName;
        response.Contacts = new() { Email = user.Email, PhoneNumber = user.PhoneNumber };

        if (profile.AvatarImageId.HasValue)
            response.AvatarUrl = await _fileStorage.GetFileUrl(profile.AvatarImageId.Value);

        return Result.Success(response);
    }
}