using Astrum.Account.Features.Achievement;
using Astrum.Account.Features.Achievement.Commands.AchievementUpdate;
using AutoMapper;

namespace Astrum.Account.Mappings;

public class AchievementProfile : Profile
{
    public AchievementProfile()
    {
        CreateMap<AchievementResponse, AchievementUpdateCommand>();
    }
}