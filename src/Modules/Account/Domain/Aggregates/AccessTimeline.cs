using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.Account.Enums;
using Astrum.SharedLib.Domain.Entities;

namespace Astrum.Account.Aggregates
{
    public class AccessTimeline : AggregateRootBase<Guid>
    {
        public Guid UserId { get; set; }
        public TimelineType TimelineType { get; set; }
        public List<AccessTimelineInterval> Intervals { get; set; }
    }
}
