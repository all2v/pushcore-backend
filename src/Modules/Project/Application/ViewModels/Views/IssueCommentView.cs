namespace Astrum.Project.Application.ViewModels.Views
{
    public class IssueCommentView
    {
        public string Id { get; set; }
        public string AuthorId { get; set; }
        public string Text { get; set; }
        public List<IssueAttachmentView> Attachments { get; set; }
    }
}
