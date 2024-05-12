using Astrum.Account.Aggregates;
using Astrum.Account.Application.Features.Achievement.Commands.AchievementRemove;
using Astrum.Account.Features.Achievement;
using Astrum.Account.Features.Achievement.Commands.AchievementAssign;
using Astrum.Account.Repositories;
using Astrum.Identity.Repositories;
using Astrum.Identity.Specifications;
using AutoMapper;

namespace Astrum.Account.Services;

public class UserAchievementService : IUserAchievementService
{
    private readonly IAchievementRepository _achievementRepository;
    private readonly IMapper _mapper;
    private readonly IUserAchievementRepository _userAchievementRepository;
    private readonly IApplicationUserRepository _userRepository;

    public UserAchievementService(IAchievementRepository achievementRepository,
        IUserAchievementRepository userAchievementRepository,
        IApplicationUserRepository userRepository,
        IMapper mapper)
    {
        _achievementRepository = achievementRepository;
        _userAchievementRepository = userAchievementRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    #region IUserAchievementService Members

    public async Task<UserAchievementResponse> AssignAchievementAsync(AchievementAssignCommand command)
    {
        var specification = new GetUserByUsernameSpec(command.Username);
        var user = await _userRepository.FirstOrDefaultAsync(specification);
        if (user == null)
            throw new Exception("User not found");
        var achievement = await _achievementRepository.GetByIdAsync(command.AchievementId);
        if (achievement == null)
            throw new Exception("Achievement not found");

        var userAchievement = new UserAchievement(user.Id);
        userAchievement.AddAchievement(achievement);

        await _userAchievementRepository.AddAsync(userAchievement);
        await _userAchievementRepository.UnitOfWork.SaveChangesAsync();

        var response = _mapper.Map<UserAchievementResponse>(userAchievement);
        return response;
    }

    #endregion

    /// <summary>
    ///     TODO DMITRY: I think it works incorrect!
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<UserAchievementResponse> RemoveAchievementAsync(AchievementRemoveCommand command)
    {
        var specification = new GetUserByUsernameSpec(command.Username);
        var user = await _userRepository.FirstOrDefaultAsync(specification);
        if (user == null)
            throw new Exception("User not found");
        var achievement = await _achievementRepository.GetByIdAsync(command.AchievementId);
        if (achievement == null)
            throw new Exception("Achievement not found");

        var userAchievement = new UserAchievement(user.Id);
        userAchievement.AddAchievement(achievement);

        await _userAchievementRepository.DeleteAsync(userAchievement);
        await _userAchievementRepository.UnitOfWork.SaveChangesAsync();

        var response = _mapper.Map<UserAchievementResponse>(userAchievement);
        return response;
    }
}