using Astrum.Account.Aggregates;
using Astrum.Account.Features.Achievement;
using AutoMapper;

namespace Astrum.Account.Mappings;

public class UserAchievementProfile : Profile
{
    public UserAchievementProfile()
    {
        CreateMap<UserAchievement, UserAchievementResponse>();
        CreateMap<UserAchievementResponse, UserAchievement>();
        CreateMap<Achievement, AchievementResponse>();
    }
}