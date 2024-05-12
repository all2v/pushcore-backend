using Astrum.Account.Features;
using Astrum.Account.Features.Registration.Commands.RegistrationApplicationUpdate;
using AutoMapper;

namespace Astrum.Account.Mappers;

public class RegistrationApplicationProfile : Profile
{
    public RegistrationApplicationProfile()
    {
        CreateMap<RegistrationApplicationUpdateRequestBody, RegistrationApplicationUpdateCommand>();
    }
}