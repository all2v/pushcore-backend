using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Specification;

namespace Astrum.Account.Specifications.SocialNetworks
{
    public class GetSocialNetworksSpec : Specification<Aggregates.SocialNetworks>
    {
        public GetSocialNetworksSpec() 
        { 
        }
    }

    public class GetSocialNetworksByIdSpec : GetSocialNetworksSpec 
    { 
        public GetSocialNetworksByIdSpec(Guid socialNetworksId) 
        {
            Query
                .Where(networks => networks.Id == socialNetworksId);
        }
    }
}
