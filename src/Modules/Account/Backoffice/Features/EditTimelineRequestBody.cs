using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.Account.Enums;
using Astrum.Account.Features.Profile.Commands;

namespace Astrum.Account.Features
{
    public class EditTimelineRequestBody
    {
        public TimelineType? TimelineType { get; set; }
        public List<EditTimelineIntervalRequestBody>? Intervals { get; set; }
    }

    public class EditTimelineIntervalRequestBody
    {
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public TimelineIntervalType? IntervalType { get; set; }
    }
}
