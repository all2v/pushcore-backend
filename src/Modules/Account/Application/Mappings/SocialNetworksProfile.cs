using Astrum.Account.Aggregates;
using Astrum.Account.Features.Profile;
using AutoMapper;

namespace Astrum.Account.Mappings;

public class SocialNetworksProfile : Profile
{
    public SocialNetworksProfile()
    {
        CreateMap<SocialNetworks, SocialNetworksResponse>();
    }
}