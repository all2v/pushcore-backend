using Astrum.Account.Aggregates;
using Astrum.Account.Features.Registration;
using AutoMapper;

namespace Astrum.Account.Mappings;

public class RegistrationApplicationProfile : Profile
{
    public RegistrationApplicationProfile()
    {
        CreateMap<RegistrationApplication, RegistrationApplicationResponse>();
    }
}