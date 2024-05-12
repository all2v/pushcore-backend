using Astrum.Account.Repositories;
using Astrum.Account.Specifications.Achievements;
using Astrum.SharedLib.Common.CQS.Implementations;
using Astrum.SharedLib.Common.Results;
using Astrum.Storage.Services;
using AutoMapper;

namespace Astrum.Account.Features.Achievement.Commands.AchievementUpdate;

public class
    AchievementUpdateCommandHandler : CommandResultHandler<AchievementUpdateCommand, AchievementResponse>
{
    private readonly IFileStorage _fileStorage;
    private readonly IAchievementRepository _achievementRepository;
    private readonly IMapper _mapper;

    public AchievementUpdateCommandHandler(IFileStorage fileStorage, IAchievementRepository achievementRepository, IMapper mapper)
    {
        _fileStorage = fileStorage;
        _achievementRepository = achievementRepository;
        _mapper = mapper;
    }

    public override async Task<Result<AchievementResponse>> Handle(AchievementUpdateCommand command,
        CancellationToken cancellationToken = default)
    {
        var achievementSpec = new GetAchievementByIdSpec(command.Id);
        var achievement = await _achievementRepository.FirstOrDefaultAsync(achievementSpec, cancellationToken);

        if (achievement == null)
            return Result.NotFound($"Достижение не найдено.");

        achievement.Name = command.Name ?? achievement.Name;
        achievement.Description = command.Description ?? achievement.Description;

        if (command.Icon != null)
        {
            try
            {
                var uploadResult = await _fileStorage.UploadFile(command.Icon, cancellationToken);
                achievement.IconId = uploadResult.UploadedFileId;
            }
            catch (Exception ex)
            {
                return Result.Error(ex.Message, "Ошибка при обновлении достижения.");
            }
        }

        await _achievementRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        var response = _mapper.Map<AchievementResponse>(achievement);
        if (achievement.IconId.HasValue)
            response.IconUrl = await _fileStorage.GetFileUrl(achievement.IconId.Value);

        return Result.Success(response);
    }
}