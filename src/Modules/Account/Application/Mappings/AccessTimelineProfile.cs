using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.Account.Aggregates;
using Astrum.Account.Features.Profile;
using AutoMapper;

namespace Astrum.Account.Mappings
{
    public class AccessTimelineProfile : Profile
    {
        public AccessTimelineProfile() 
        {
            CreateMap<AccessTimeline, AccessTimelineResponse>();
            CreateMap<AccessTimelineInterval, AccessTimelineIntervalResponse>();
        }
    }
}
