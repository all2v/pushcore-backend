using Ardalis.Specification;
using Astrum.ITDictionary.Aggregates;

namespace Astrum.ITDictionary.Specifications;

public class GetTermSpec : Specification<Term>
{
    public GetTermSpec()
    {
        Query
            .Include(t => t.Category);
    }
}

public class GetTermByIdSpec : GetTermSpec
{
    public GetTermByIdSpec(Guid id)
    {
        Query
            .Where(t => t.Id == id);
    }
}

public class GetTermsByIdsSpec : GetTermSpec
{
    public GetTermsByIdsSpec(List<Guid> termIds)
    {
        // TODO тут должно проецирование массива на массив
        Query
            .Where(t => termIds.Contains(t.Id));
    }
}

public class GetTermsByCategoryIdSpec : GetTermSpec
{
    public GetTermsByCategoryIdSpec(Guid categoryId)
    {
        Query
            .Where(t => t.CategoryId == categoryId);
    }
}

public class GetTermsBySubstringSpec : GetTermSpec
{
    public GetTermsBySubstringSpec(string substring)
    {
        substring = substring.ToLower();
        Query
            .Where(t => t.Name.ToLower().Contains(substring) ||
                        t.Category.Name.ToLower().Contains(substring) ||
                        t.Definition.ToLower().Contains(substring));
    }
}