namespace Astrum.Project.Application.ViewModels.Views
{
    public class IssueView
    {
        public string Summary { get; set; }
        public string Description { get; set; }
        //TODO: переделать под профили
        public string ReporterId { get; set; }
        public string UpdaterId { get; set; }
        public string DraftOwnerId { get; set; }
        public bool? IsDraft { get; set; }
        public string Url { get; set; }
        public List<IssueCommentView> Comments { get; set; }
        public int? CommentsCount { get; set; }
        public List<IssueTagView> Tags { get; set; }
        public List<string> WatcherIds { get; set; }
        public List<IssueAttachmentView> Attachments { get; set; }
        public List<IssueView> Subtasks { get; set; }
        public IssueView Parent { get; set; }
    }
}
