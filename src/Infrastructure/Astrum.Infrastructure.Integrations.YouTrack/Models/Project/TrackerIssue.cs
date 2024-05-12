namespace Astrum.Infrastructure.Integrations.YouTrack.Models.Project
{
    public class TrackerIssue
    {
        public string Id { get; set; }
        public TrackerProject Project { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public SynchronisationUser Reporter { get; set; }
        public SynchronisationUser Updater { get; set; }
        public SynchronisationUser DraftOwner { get; set; }
        public string Url { get; set; }
        public bool? IsDraft { get; set; }
        public bool IsNew { get; set; }
        public List<TrackerIssueComment> Comments { get; set; }
        public int? CommentsCount { get; set; }
        public List<TrackerIssueTag> Tags { get; set; }
        public List<string> WatcherIds { get; set; }
        public List<TrackerAttachment> Attachments { get; set; }
        public List<TrackerIssue> Subtasks { get; set; }
        public List<IssueCustomField>? CustomFields { get; set; }
        public TrackerIssue Parent { get; set; }
        public long? Updated { get; set; }
        public DateTime UpdatedDate { get; set; }
        public long? Resolved { get; set; }
        public DateTime ResolvedDate { get; set; }
    }
}