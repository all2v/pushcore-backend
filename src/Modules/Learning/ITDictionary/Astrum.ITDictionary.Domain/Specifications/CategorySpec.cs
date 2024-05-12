using Ardalis.Specification;
using Astrum.ITDictionary.Aggregates;

namespace Astrum.ITDictionary.Specifications;

public class GetCategorySpec : Specification<Category>
{
    public GetCategorySpec()
    {
        Query
            .OrderBy(c => c.Name);
    }
}

public class GetCategoryByIdSpec : GetCategorySpec
{
    public GetCategoryByIdSpec(Guid id)
    {
        Query
            .Where(c => c.Id == id);
    }
}

public class GetCategoriesBySubstringSpec : GetCategorySpec
{
    public GetCategoriesBySubstringSpec(string substring)
    {
        Query
            .Where(c => c.Name.ToLower().Contains(substring.ToLower()));
    }
}