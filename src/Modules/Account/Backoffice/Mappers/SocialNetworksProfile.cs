using Astrum.Account.Features;
using Astrum.Account.Features.Profile.Commands;
using AutoMapper;

namespace Astrum.Account.Mappers;

public class SocialNetworksProfile : Profile
{
    public SocialNetworksProfile()
    {
        CreateMap<EditSocialNetworksRequestBody, EditSocialNetworksCommand>();
    }
}