using Ardalis.Specification;
using Astrum.Articles.Aggregates;
using Astrum.Articles.Repositories;
using Astrum.SharedLib.Persistence.Repositories;

namespace Astrum.Articles.Repositories
{
    public class ArticleTagRepository : EFRepository<ArticleTag, Guid, ArticlesDbContext>, IArticleTagRepository
    {
        public ArticleTagRepository(ArticlesDbContext context, ISpecificationEvaluator? specificationEvaluator = null) : base(
            context, specificationEvaluator)
        {
        }
    }
}