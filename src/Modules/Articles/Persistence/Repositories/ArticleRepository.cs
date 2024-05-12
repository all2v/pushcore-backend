using Ardalis.Specification;
using Astrum.Articles.Aggregates;
using Astrum.SharedLib.Persistence.Repositories;

namespace Astrum.Articles.Repositories;

public class ArticleRepository : EFRepository<Article, Guid, ArticlesDbContext>, IArticleRepository
{
    public ArticleRepository(ArticlesDbContext context, ISpecificationEvaluator? specificationEvaluator = null) : base(
        context, specificationEvaluator)
    {
    }
}