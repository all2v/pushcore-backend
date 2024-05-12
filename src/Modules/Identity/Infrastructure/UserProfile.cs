using Astrum.Identity.Models;
using Astrum.Identity.ReadModels;
using Astrum.SharedLib.Common.Extensions;
using Astrum.SharedLib.Domain.Enums;
using AutoMapper;

namespace Astrum.Identity;

internal class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<ApplicationUserRole, RolesEnum>()
            .ConvertUsing(r => r.Role.Name.ToEnum<RolesEnum>());

        // CreateMap<User, ApplicationUser>()
        // .ForMember(target => target.Roles, opt => opt.Ignore())
        // .ForPath(target => target.PhoneNumber, source => source.MapFrom(m => m.PrimaryPhoneNumber));

        // CreateMap<ApplicationUser, User>()
        // .ForMember(target => target.PrimaryPhoneNumber, source => source.MapFrom(m => m.PhoneNumber));

        CreateMap<ApplicationUser, UserReadModel>();
        // CreateMap<IdentityError, ResultError>()
            // .ForMember(target => target.ExceptionMessage, opt => opt.MapFrom(source => source.Description));
    }
}