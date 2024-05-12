using Astrum.Account.Features.Account.AccountDetails;
using Astrum.Account.Features.Account.UserEdit.Commands;
using Astrum.Identity.Mappings.Resolvers;
using Astrum.Identity.Models;
using Astrum.Identity.ReadModels;
using Astrum.IdentityServer.Domain.ViewModels;
using AutoMapper;

namespace Astrum.Account.Mappings;

public class AccountProfile : Profile
{
    public AccountProfile()
    {
        CreateMap<UserReadModel, UserEditCommand>();
        CreateMap<UserReadModel, AccountDetailsQuery>();
        CreateMap<ApplicationUser, UserReadModel>()
            .ForMember(target => target.LocalizedRoles, source => source.MapFrom<LocalizedRolesResolver>());

        CreateMap<ApplicationUser, UserViewModel>();
        // CreateMap<string, PhoneNumber>().ConvertUsing<StringToPhoneNumberConverter>();
        // CreateMap<PhoneNumber, string>().ConvertUsing<PhoneNumberToStringConverter>();
    }
}