using Astrum.SharedLib.Common.Results;
using Astrum.Identity.DTOs;
using Astrum.Identity.Features.Commands.Login;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace Astrum.Identity.Mappings;

public class IdentityProfile : Profile
{
    public IdentityProfile()
    {
        CreateMap<LoginCommand, LoginRequestDTO>();
        CreateMap<IdentityResult, Result>();
        // CreateMap<string, PhoneNumber>().ConvertUsing<StringToPhoneNumberConverter>();
        // CreateMap<PhoneNumber, string>().ConvertUsing<PhoneNumberToStringConverter>();
    }
}