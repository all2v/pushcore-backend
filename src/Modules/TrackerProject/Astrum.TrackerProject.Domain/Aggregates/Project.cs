using System.ComponentModel.DataAnnotations.Schema;
using Astrum.SharedLib.Domain.Entities;

namespace Astrum.TrackerProject.Domain.Aggregates
{
    public class Project : AggregateRootBase<string>
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? LeaderId { get; set; }
        public List<Issue> Issues { get; set; } = new List<Issue>();
        //public string FromEmail { get; set; }
        //public string ReplyToEmail { get; set; }
        public string? IconUrl { get; set; }
        [ForeignKey("Team")]
        public string TeamId { get; set; }
        public Team Team { get; set; }
    }
}
