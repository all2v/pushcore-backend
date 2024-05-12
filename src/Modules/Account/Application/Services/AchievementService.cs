using Astrum.Account.Aggregates;
using Astrum.Account.Features.Achievement;
using Astrum.Account.Features.Achievement.Commands.AchievementCreate;
using Astrum.Account.Features.Achievement.Commands.AchievementUpdate;
using Astrum.Account.Repositories;
using Astrum.Account.Specifications.Achievements;
using Astrum.Identity.Repositories;
using Astrum.Identity.Specifications;
using Astrum.SharedLib.Common.Results;
using AutoMapper;

namespace Astrum.Account.Services;

public class AchievementService : IAchievementService
{
    private readonly IAchievementRepository _achievementRepository;
    private readonly IMapper _mapper;
    private readonly IUserAchievementRepository _userAchievementRepository;
    private readonly IApplicationUserRepository _userRepository;

    public AchievementService(IAchievementRepository achievementRepository,
        IUserAchievementRepository userAchievementRepository,
        IApplicationUserRepository userRepository,
        IMapper mapper)
    {
        _achievementRepository = achievementRepository;
        _userAchievementRepository = userAchievementRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    #region IAchievementService Members

    public async Task<Result<AchievementResponse>> CreateAsync(AchievementCreateCommand command)
    {
        var achievement = new Achievement
        {
            // IconUrl = command.IconUrl,
            Name = command.Name,
            Description = command.Description
        };
        await _achievementRepository.AddAsync(achievement);
        try
        {
            await _achievementRepository.UnitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message, "Ошибка при создании достижения.");
        }
        var response = _mapper.Map<AchievementResponse>(achievement);
        return Result.Success(response);
    }

    public async Task<Result<AchievementResponse>> GetAsync(Guid achievementId)
    {
        var getAchievementByIdSpec = new GetAchievementByIdSpec(achievementId);
        var achievement = await _achievementRepository.FirstOrDefaultAsync(getAchievementByIdSpec);
        if (achievement == null)
            return Result.NotFound("Достижение не найдено.");

        var response = _mapper.Map<AchievementResponse>(achievement);
        return response;
    }

    public async Task<Result<List<AchievementResponse>>> GetUserAchievementsByUsernameAsync(string username)
    {
        var getUserByUsernameSpecification = new GetUserByUsernameSpec(username);
        var user = await _userRepository.FirstOrDefaultAsync(getUserByUsernameSpecification);
        if (user == null)
        {
            return Result.NotFound("Пользователь не найден.");
        }
        var getUserAchievementsByUserIdSpec = new GetUserAchievementsByUserIdSpec(user.Id);
        var userAchievements = await _userAchievementRepository.ListAsync(getUserAchievementsByUserIdSpec);
        var achievements = userAchievements.Select(x => x.Achievements).ToList();

        var response = _mapper.Map<List<AchievementResponse>>(achievements);
        return Result.Success(response);
    }

    public async Task<Result<AchievementResponse>> DeleteAsync(Guid achievementId)
    {
        var getAchievementByIdSpec = new GetAchievementByIdSpec(achievementId);
        var achievement = await _achievementRepository.FirstOrDefaultAsync(getAchievementByIdSpec);
        if (achievement == null)
            return Result.NotFound("Достижение не найдено.");

        try
        {
            await _achievementRepository.DeleteAsync(achievement);
            await _achievementRepository.UnitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message, "Ошибка при удалении достижения.");
        }
        var response = _mapper.Map<AchievementResponse>(achievement);
        return Result.Success(response);
    }

    public async Task<Result<AchievementResponse>> UpdateAsync(AchievementUpdateCommand command)
    {
        var getAchievementByIdSpec = new GetAchievementByIdSpec(command.Id);
        var achievement = await _achievementRepository.FirstOrDefaultAsync(getAchievementByIdSpec);
        if (achievement == null)
            return Result.NotFound("Достижение не найдено.");
        // achievement.IconUrl = command.IconUrl;
        achievement.Name = command.Name;
        achievement.Description = command.Description;
        try
        {
            await _achievementRepository.UnitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message, "Ошибка при обновлении достижения.");
        }

        var response = _mapper.Map<AchievementResponse>(achievement);
        return Result.Success(response);
    }

    #endregion
}