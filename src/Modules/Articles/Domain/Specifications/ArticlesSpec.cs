using Ardalis.Specification;
using Astrum.Articles.Aggregates;

namespace Astrum.Articles.Specifications;

public class GetArticlesWithIncludesSpec : Specification<Article>
{
    public GetArticlesWithIncludesSpec()
    {
        Query
            .Include(x=>x.Author)
            .Include(x=>x.Category)
            .Include(x=>x.Tags);
    }
}

public class GetArticlesDescSpec : GetArticlesWithIncludesSpec
{
    public GetArticlesDescSpec()
    {
        Query
            .Include(x => x.Tags)
            .OrderByDescending(x => x.DateCreated);
    }
}
public class GetArticleByIdSpec : GetArticlesWithIncludesSpec
{
    public GetArticleByIdSpec(Guid id)
    {
        Query
            .Where(x => x.Id == id);
        
    }
}
public class GetArticlesByAuthorIdSpec : GetArticlesWithIncludesSpec
{
    public GetArticlesByAuthorIdSpec(Guid authorId)
    {
        Query
            .Where(x => x.Author.UserId == authorId);
        
    }
}

public class GetArticlesByCategoryIdSpec : GetArticlesWithIncludesSpec
{
    public GetArticlesByCategoryIdSpec(Guid categoryId)
    {
        Query
            .Where(x => x.CategoryId == categoryId);

    }
}

public class GetArticleBySlugSpec : GetArticlesWithIncludesSpec
{
    public GetArticleBySlugSpec(ArticleSlug slug)
    {
        Query
            .Where(x => x.Slug.AuthorSlug == slug.AuthorSlug 
                && x.Slug.NameSlug == slug.NameSlug);

    }
}

public class GetArticleByTrackerIdSpec : GetArticlesWithIncludesSpec
{
    public GetArticleByTrackerIdSpec(string id)
    {
        Query
            .Where(x => x.TrackerArticleId == id);

    }
}