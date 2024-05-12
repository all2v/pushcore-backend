using Astrum.Account.Features.Profile;
using Astrum.TrackerProject.Domain.Aggregates;

namespace Astrum.TrackerProject.Application.ViewModels
{
    public class IssueForm
    {
        public string Id { get; set; }
        public ProjectForm Project { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public string ReporterId { get; set; }
        public string UpdaterId { get; set; }
        public string DraftOwnerId { get; set; }
        public UserProfileSummary Reporter { get; set; }
        public UserProfileSummary Updater { get; set; }
        public UserProfileSummary DraftOwner { get; set; }
        public string? Priority { get; set; }
        public string? State { get; set; }
        public string? AssigneeId { get; set; }
        public UserProfileSummary Assignee { get; set; }
        public string Url { get; set; }
        public bool? IsDraft { get; set; }
        public List<IssueCommentForm> Comments { get; set; }
        public int? CommentsCount { get; set; }
        public List<IssueTagForm> Tags { get; set; }
        public List<string> WatcherIds { get; set; }
        public List<AttachmentForm> Attachments { get; set; }
        public List<IssueForm> Subtasks { get; set; }
        public IssueForm Parent { get; set; }
        public DateTime Updated { get; set; }
        public DateTime Resolved { get; set; }
    }
}
