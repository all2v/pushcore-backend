using Ardalis.Specification;
using Astrum.Appeal.Aggregates;

namespace Astrum.Appeal.Specifications;

public class GetCategoriesSpecNoTracking : Specification<AppealCategory>
{
    public GetCategoriesSpecNoTracking(IEnumerable<Guid> ids)
    {
        Query
            .AsNoTracking()
            .Where(x => ids.Contains(x.Id));
    }
}

public class GetCategoriesSpec : Specification<AppealCategory> { }

public class GetCategoryById : GetCategoriesSpec
{
    public GetCategoryById(Guid id)
    {
        Query
            .Where(x => x.Id == id);
    }
}