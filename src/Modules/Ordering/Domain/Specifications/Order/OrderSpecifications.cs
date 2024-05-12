using Ardalis.Specification;

namespace Astrum.Ordering.Specifications.Order;

public class GetOrdersSpec : Specification<Aggregates.Order>
{
    public GetOrdersSpec()
    {
        Query
            .Include(x => x.OrderItems);
    }
}

public class GetOrderByIdSpec : GetOrdersSpec
{
    public GetOrderByIdSpec(Guid id)
    {
        Query
            .Where(x => x.Id == id);
    }
}