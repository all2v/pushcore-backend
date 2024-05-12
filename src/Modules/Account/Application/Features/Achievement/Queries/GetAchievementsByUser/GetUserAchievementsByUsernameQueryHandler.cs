using Astrum.Account.Repositories;
using Astrum.Account.Services;
using Astrum.Account.Specifications.UserProfile;
using Astrum.Identity.Repositories;
using Astrum.Identity.Specifications;
using Astrum.SharedLib.Common.CQS.Implementations;
using Astrum.SharedLib.Common.Results;
using Astrum.Storage.Services;
using AutoMapper;

namespace Astrum.Account.Features.Achievement.Queries.GetAchievementsByUser;

public class
    GetUserAchievementsByUsernameQueryHandler : QueryResultHandler<GetUserAchievementsByUsernameQuery,
        List<AchievementResponse>>
{
    private readonly IUserProfileRepository _profileRepository;
    private readonly IMapper _mapper;
    private readonly IFileStorage _fileStorage;
    private readonly IApplicationUserRepository _userRepository;

    public GetUserAchievementsByUsernameQueryHandler(IUserProfileRepository profileRepository, IMapper mapper,
        IApplicationUserRepository userRepository, IFileStorage fileStorage)
    {
        _profileRepository = profileRepository;
        _mapper = mapper;
        _fileStorage = fileStorage;
        _userRepository = userRepository;
    }

    public override async Task<Result<List<AchievementResponse>>> Handle(GetUserAchievementsByUsernameQuery query,
        CancellationToken cancellationToken = default)
    {
        var userSpec = new GetUserByUsernameSpec(query.Username);
        var user = await _userRepository.FirstOrDefaultAsync(userSpec, cancellationToken);
        if (user == null)
            return Result.NotFound("Пользователь не найден.");
        
        var profileSpec = new GetUserProfileByUserIdSpec(user.Id);
        var profile = await _profileRepository.FirstOrDefaultAsync(profileSpec, cancellationToken);
        if (profile == null)
            return Result.NotFound("Профиль пользователя не найден.");
        
        var achievements = _mapper.Map<List<AchievementResponse>>(profile.Achievements);
        foreach(var achievement in achievements)
        {
            if (achievement.IconId.HasValue)
                achievement.IconUrl = await _fileStorage.GetFileUrl(achievement.IconId.Value);
        }
        
        return Result.Success(achievements);
    }
}