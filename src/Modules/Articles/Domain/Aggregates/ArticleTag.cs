using Astrum.SharedLib.Domain.Entities;

namespace Astrum.Articles.Aggregates
{
    public class ArticleTag : AggregateRootBase<Guid>
    {
        public Guid TagId { get; set; }
        public Guid ArticleId { get; set; }      
        public ArticleTag() { }
        public ArticleTag(Guid tagId, Guid articleId)
        {
            TagId = tagId;
            ArticleId = articleId;
        }
    }
}
