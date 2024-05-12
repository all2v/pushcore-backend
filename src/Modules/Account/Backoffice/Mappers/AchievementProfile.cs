using Astrum.Account.Features;
using Astrum.Account.Features.Achievement.Commands.AchievementUpdate;
using AutoMapper;

namespace Astrum.Account.Mappers;

public class AchievementProfile : Profile
{
    public AchievementProfile()
    {
        CreateMap<AchievementUpdateRequestBody, AchievementUpdateCommand>();
    }
}