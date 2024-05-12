using Astrum.SharedLib.Domain.Entities;

namespace Astrum.Articles.Aggregates
{
    public class Tag : AggregateRootBase<Guid>
    {
        private Tag() { }
        public Tag(Guid id)
        {
            Id = id;
        }

        public string Name { get; set; }
        public Guid CategoryId { get; set; }
        public List<ArticleTag> ArticleTags { get; set; }
    }
}
