using Ardalis.Specification;
using Astrum.Articles.Aggregates;

namespace Astrum.Articles.Specifications;

public class GetInfoWithIncludesSpec : Specification<Category>
{
    public GetInfoWithIncludesSpec()
    {
        Query
            .Include(c=>c.Tags)
            .ThenInclude(t=>t.ArticleTags)
            .AsSplitQuery();
    }
}
