using Ardalis.Specification;
using Astrum.Projects.Aggregates;

namespace Astrum.Projects.Specifications.Customer;

public class GetProductsSpec : Specification<Product>
{
    public GetProductsSpec()
    {
        Query
            .Include(x => x.Customer)
            .Include(x => x.Projects);
    }
}

public class GetProductByIdSpec : GetProductsSpec
{
    public GetProductByIdSpec(Guid id)
    {
        Query
            .Where(x => x.Id == id)
            .Include(x => x.Projects)
            .ThenInclude(x => x.Members);
    }
}

public class GetProductsPaginateSpec : GetProductsSpec
{
    public GetProductsPaginateSpec(int count, int startIndex)
    {
        Query
            .OrderBy(e => e.Index)
            .Where(e => e.Index > startIndex)
            .Take(count);
    }

    public GetProductsPaginateSpec(int count, int startIndex, string? predicate)
    {
        Query
            .Where(e=>e.Name.ToUpper().Contains(predicate.ToUpper())
                || e.Customer.Name.ToUpper().Contains(predicate.ToUpper()))
            .OrderBy(e => e.Index)
            .Where(e => e.Index > startIndex)
            .Take(count);
    }
}

public class CheckNextSpec : Specification<Product>
{
    public CheckNextSpec(int index)
    {
        Query
            .OrderBy(e => e.Index)
            .Where(e => e.Index > index);
    }
}