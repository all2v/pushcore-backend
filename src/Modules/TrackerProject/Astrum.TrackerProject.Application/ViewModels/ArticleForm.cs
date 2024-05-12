using Astrum.Account.Features.Profile;

namespace Astrum.TrackerProject.Application.ViewModels
{
    public class ArticleForm
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string AuthorId { get; set; }
        public UserProfileSummary Author { get; set; }
        public ProjectForm Project { get; set; }
        public ArticleForm ParentArticle { get; set; }
        public List<ArticleForm> ChildArticles { get; set; }
        public List<ArticleCommentForm> Comments { get; set; }
        public List<AttachmentForm> Attachments { get; set; }
        public bool IsNew { get; set; }
    }
}
