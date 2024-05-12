using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrum.Infrastructure.Integrations.YouTrack.Models.Project
{
    public class TrackerProject
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public SynchronisationUser Leader { get; set; }
        //public string Leader { get; set; }
        public List<TrackerIssue> Issues { get; set; }
        public string IconUrl { get; set; }
        //public string TeamId { get; set; }
        //public string TeamRingId { get; set; }
        public TrackerProjectTeam Team { get; set; }

    }
}
