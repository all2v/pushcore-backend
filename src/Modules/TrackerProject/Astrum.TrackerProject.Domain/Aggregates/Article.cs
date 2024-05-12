using Astrum.Account.Features.Profile;
using Astrum.SharedLib.Domain.Entities;

namespace Astrum.TrackerProject.Domain.Aggregates
{
    public class Article : AggregateRootBase<string>
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public string Content { get; set; }
        public Article? Parent { get; set; }
        public string? AuthorId { get; set; }
        public string? ProjectId { get; set; }
        public string? ParentArticleId { get; set; } 
        public List<string>? ChildArticles { get; set; }
        public List<ArticleComment>? Comments { get; set; }
        public List<Attachment>? Attachments { get; set; }
        public bool IsNew { get; set; }
    }
}
