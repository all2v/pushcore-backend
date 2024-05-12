using Astrum.Account.Repositories;
using Astrum.Account.Services;
using Astrum.Account.Specifications.Achievements;
using Astrum.Account.Specifications.UserProfile;
using Astrum.Identity.Repositories;
using Astrum.Identity.Specifications;
using Astrum.SharedLib.Common.CQS.Implementations;
using Astrum.SharedLib.Common.Results;
using AutoMapper;

namespace Astrum.Account.Features.Achievement.Commands.AchievementAssign;

public class
    AchievementAssignCommandHandler : CommandResultHandler<AchievementAssignCommand, UserAchievementResponse>
{
    private readonly IAchievementRepository _achievementRepository;
    private readonly IMapper _mapper;
    private readonly IUserAchievementRepository _userAchievementRepository;
    private readonly IApplicationUserRepository _userRepository;
    private readonly IUserProfileRepository _profileRepository;

    public AchievementAssignCommandHandler(IAchievementRepository achievementRepository,
        IUserAchievementRepository userAchievementRepository,
        IApplicationUserRepository userRepository,
        IMapper mapper, IUserProfileRepository profileRepository)
    {
        _achievementRepository = achievementRepository;
        _userAchievementRepository = userAchievementRepository;
        _userRepository = userRepository;
        _mapper = mapper;
        _profileRepository = profileRepository;
    }

    public override async Task<Result<UserAchievementResponse>> Handle(AchievementAssignCommand command,
        CancellationToken cancellationToken = default)
    {
        var specification = new GetUserByUsernameSpec(command.Username);
        var user = await _userRepository.FirstOrDefaultAsync(specification);
        if (user == null)
            return Result.NotFound("Пользователь не найден.");

        var profileSpec = new GetUserProfileByUserIdSpec(user.Id);
        var profile = await _profileRepository.FirstOrDefaultAsync(profileSpec, cancellationToken);
        if (profile == null)
            return Result.NotFound("Профиль пользователя не найден.");

        var achievementSpec = new GetAchievementByIdSpec(command.AchievementId);
        var achievement = await _achievementRepository.FirstOrDefaultAsync(achievementSpec, cancellationToken);
        if (achievement == null)
            return Result.NotFound("Достижение не найдено.");

        var userAlreadyHasAchievement = profile.Achievements.Contains(achievement);
        if (userAlreadyHasAchievement)
            return Result.Error("Пользователь уже получил это достижение.");

        profile.Achievements.Add(achievement);
        try
        {
            await _profileRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message, "Ошибка при присвоении достижения пользователю.");
        }

        return Result.Success(new UserAchievementResponse
        {
            AchievementId = command.AchievementId,
            UserId = user.Id
        });
        // var achievement = await _userAchievementService.AssignAchievementAsync(command);
        // return Result.Success(achievement);
    }
}