using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.Account.Enums;

namespace Astrum.Account.Features.Profile
{
    public class AccessTimelineIntervalResponse
    {
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public TimelineIntervalType IntervalType { get; set; }
    }
}
