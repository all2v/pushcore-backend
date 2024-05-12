using Astrum.Account.Features;
using Astrum.Account.Features.Profile.Commands;
using AutoMapper;

namespace Astrum.Account.Mappers;

public class UserProfileProfile : Profile
{
    public UserProfileProfile()
    {
        CreateMap<EditUserProfileRequestBody, EditUserProfileCommand>()
            .ForMember(dest => dest.Username, opt => opt.Ignore())
            .ForMember(dest => dest.NewUsername, opt => opt.MapFrom(src => src.Username))
            .ForMember(dest => dest.Contacts, opt => opt.MapFrom(src => src.Contacts))
            .ForMember(dest => dest.SocialNetworks, opt => opt.MapFrom(src => src.SocialNetworks))
            .ForMember(dest => dest.Timelines, opt => opt.MapFrom(src => src.Timelines))
            .ForMember(d => d.ActiveTimeline,
                o => o.MapFrom(s => s.Timelines.FirstOrDefault(x => x.TimelineType == s.ActiveTimeline.TimelineType)));

        CreateMap<EditTimelineRequestBody, EditTimelineCommand>()
            .ForMember(dest => dest.Intervals, opt => opt.MapFrom(src => src.Intervals));
        CreateMap<EditTimelineIntervalRequestBody, EditTimelineIntervalCommand>();
    }
}