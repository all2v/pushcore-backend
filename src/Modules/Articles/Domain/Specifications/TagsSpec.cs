
using Ardalis.Specification;
using Astrum.Articles.Aggregates;

namespace Astrum.Articles.Specifications;

public class TagsByCategoryIdSpec : Specification<Tag>
{
    public TagsByCategoryIdSpec(Guid categoryId)
    {
        Query
            .Where(e=>e.CategoryId == categoryId);
    }
}

public class TagsByNameSpec : Specification<Tag>
{
    public TagsByNameSpec(string name)
    {
        Query
            .Where(e => e.Name.ToLower() == name.ToLower());
    }
}

public class TagsByCountAndPredicateSpec : Specification<Tag>
{
    public TagsByCountAndPredicateSpec(int count, string predicate)
    {
        if(!string.IsNullOrWhiteSpace(predicate))
            Query.Where(e => e.Name.ToLower().Trim().Contains(predicate.ToLower().Trim()));
        Query.Take(count);
    }
}
