namespace Astrum.Infrastructure.Integrations.YouTrack.Models.TrackerDTO.IssueDTO
{
    public class IssueEvent
    {
        public bool? IsNew { get; set; }

        public bool? IsResolved { get; set; }

        public Issue Issue { get; set; }

        public DateTime DateTime { get; set; }

        public User CreatedBy { get; set; }

        public User UpdatedBy { get; set; }

        public User Assignee { get; set; }

        public Project Project { get; set; }

        public List<Change> Changes { get; set; }

        public List<Comment> Comments { get; set; }

    }
}
