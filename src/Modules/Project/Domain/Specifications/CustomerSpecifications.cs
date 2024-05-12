using Ardalis.Specification;

namespace Astrum.Projects.Specifications.Customer;

public class GetCustomersSpec : Specification<Aggregates.Customer>
{
}

public class GetCustomerByIdSpec : GetCustomersSpec
{
    public GetCustomerByIdSpec(Guid id)
    {
        Query
            .Where(x => x.Id == id);
    }
}

public class GetCustomerByNameSpec : GetCustomersSpec
{
    public GetCustomerByNameSpec(string name)
    {
        Query
            .Where(x => x.Name.ToLower() == name.ToLower());
    }
}