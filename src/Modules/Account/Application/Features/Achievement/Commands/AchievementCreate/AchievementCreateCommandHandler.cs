using Astrum.Account.Repositories;
using Astrum.SharedLib.Common.CQS.Implementations;
using Astrum.SharedLib.Common.Results;
using Astrum.Storage.Services;
using AutoMapper;

namespace Astrum.Account.Features.Achievement.Commands.AchievementCreate;

public sealed class
    AchievementCreateCommandHandler : CommandResultHandler<AchievementCreateCommand, AchievementResponse>
{
    private readonly IFileStorage _fileStorage;
    private readonly IAchievementRepository _achievementRepository;
    private readonly IMapper _mapper;

    public AchievementCreateCommandHandler(IAchievementRepository achievementRepository, IFileStorage fileStorage, IMapper mapper)
    {
        _achievementRepository = achievementRepository;
        _fileStorage = fileStorage;
        _mapper = mapper;
    }

    public override async Task<Result<AchievementResponse>> Handle(AchievementCreateCommand command,
        CancellationToken cancellationToken = default)
    {
        var achievement = new Aggregates.Achievement()
        {
            Name = command.Name,
            Description = command.Description
        };

        if (command.Icon != null)
        {
            var uploadResult = await _fileStorage.UploadFile(command.Icon);
            achievement.IconId = uploadResult.UploadedFileId;
        }

        achievement = await _achievementRepository.AddAsync(achievement);
        try
        {
            await _achievementRepository.UnitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message, "Ошибка при создании достижения.");
        }

        var response = _mapper.Map<AchievementResponse>(achievement);
        if (achievement.IconId.HasValue)
            response.IconUrl = await _fileStorage.GetFileUrl(achievement.IconId.Value);

        return Result.Success(response);
    }
}