using Astrum.SharedLib.Domain.Entities;

namespace Astrum.TrackerProject.Domain.Aggregates
{
    public class ArticleComment : AggregateRootBase<string>
    {
        public string AuthorId { get; set; }
        public string? Text { get; set; }
        public List<Attachment> Attachments { get; set; }
    }
}
