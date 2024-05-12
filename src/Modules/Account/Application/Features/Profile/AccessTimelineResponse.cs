using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.Account.Enums;

namespace Astrum.Account.Features.Profile
{
    public class AccessTimelineResponse
    {
        public TimelineType TimelineType { get; set; }
        public List<AccessTimelineIntervalResponse> Intervals { get; set; }
    }
}
