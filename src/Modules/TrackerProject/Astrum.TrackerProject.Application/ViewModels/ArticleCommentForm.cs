using Astrum.Account.Features.Profile;

namespace Astrum.TrackerProject.Application.ViewModels
{
    public class ArticleCommentForm
    {
        public string Id { get; set; }
        public string AuthorId { get; set; }
        public UserProfileSummary Author { get; set; }
        public string Text { get; set; }
        public List<AttachmentForm> Attachments { get; set; }
    }
}
