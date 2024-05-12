using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.Account.Enums;
using Astrum.SharedLib.Domain.Entities;

namespace Astrum.Account.Aggregates
{
    public class AccessTimelineInterval : AggregateRootBase<Guid>
    {
        public Guid TimelineId { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public TimelineIntervalType IntervalType { get; set; }
    }
}
