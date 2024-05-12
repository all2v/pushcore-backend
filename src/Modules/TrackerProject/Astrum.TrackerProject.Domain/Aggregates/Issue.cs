using Astrum.Account.Aggregates;
using Astrum.SharedLib.Domain.Entities;

namespace Astrum.TrackerProject.Domain.Aggregates
{
    public class Issue : AggregateRootBase<string>
    {
        public Project Project { get; set; }
        public string ProjectId { get; set; }
        public string? Summary { get; set; }
        public string? Description { get; set; }
        public string? ReporterId { get; set; }
        public string? UpdaterId { get; set; }
        public string? DraftOwnerId { get; set; }
        public string? Url { get; set; }
        public bool? IsDraft { get; set; }
        public string? Priority { get; set; }
        public string? State { get; set; }
        public string? AssigneeId { get; set; }
        public ExternalUser? Assignee { get; set; }
        public List<IssueComment> Comments { get; set; } = new List<IssueComment>();
        public int? CommentsCount { get; set; }
        //public List<UserProfile> Watchers { get; set; }
        public List<Attachment> Attachments { get; set; }
        public List<Issue> Subtasks { get; set; }
        public Issue? Parent { get; set; }
        public DateTime Updated { get; set; }
        public DateTime Resolved { get; set; }
    }
}
