using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.Account.Aggregates;
using Astrum.Account.Application.Features.Profile;
using Astrum.Account.Application.Features.Profile.Commands;
using Astrum.Account.Features.Account.AccountDetails;
using Astrum.Account.Features.Account.UserEdit.Commands;
using Astrum.Account.Features.Profile;
using Astrum.Identity.DTOs;
using Astrum.Identity.Features.Commands;
using Astrum.Identity.ReadModels;
using Astrum.News.DomainServices.ViewModels.Responces;
using Astrum.News.ViewModels;
using AutoMapper;

namespace Astrum.Account.Mappings
{
    public class UserProfileProfile : Profile
    {
        public UserProfileProfile() 
        {
            CreateMap<UserProfile, UserProfileResponse>()
                .ForMember(src => src.Position, opt => opt.MapFrom(src => src.Position.Name))
                .ForMember(d => d.ActiveTimeline,
                    o => o.MapFrom(s => s.Timelines.FirstOrDefault(x => x.Id == s.ActiveTimeline)));
            CreateMap<UserProfile, EditUserProfileResponse>()
                .ForMember(d => d.Position, o => o.MapFrom(s => s.Position.Name))
                .ForMember(d => d.ActiveTimeline,
                    o => o.MapFrom(s => s.Timelines.FirstOrDefault(x => x.Id == s.ActiveTimeline)));
            CreateMap<UserProfile, UserProfileSummary>()
                .ForMember(d => d.PositionName, o => o.MapFrom(s => s.Position.Name))
                .ForMember(destination => destination.BirthDate, 
                    options => options.MapFrom(source => source.BirthDate.Value.ToDateTime(TimeOnly.MinValue)));
            CreateMap<UserProfileSummary, UserInfo>();
            CreateMap<UserEditCommand, UpdateUserDetailsCommand>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id.ToString()))
                .ForMember(d => d.Name, o => o.MapFrom(s => $"{s.LastName} {s.FirstName} {s.Patronymic}"));
            CreateMap<UserProfileSummary, UserEditCommand>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.UserId))
                .ForMember(d => d.FirstName, o => o.MapFrom(s => s.Name))
                .ForMember(d => d.LastName, o => o.MapFrom(s => s.Surname))
                .ForMember(d => d.BirthDate, o => o.Ignore()).ReverseMap();
            CreateMap<UpdateUserDetailsCommand, UpdateUserDetailsDto>();
            CreateMap<UserEditCommand, EditProfileCommand>()
                .ForMember(d => d.UserId, o => o.MapFrom(s => s.Id));
            CreateMap<UserReadModel, EditUserResponse>();
            CreateMap<UserProfileResponse, BasicUserInfoResponse>();
            CreateMap<UserProfileResponse, FullNameResponce>();
            CreateMap<UserProfileSummary, UserResponse>();
        }
    }
}
