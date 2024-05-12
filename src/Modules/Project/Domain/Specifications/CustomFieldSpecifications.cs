using Ardalis.Specification;
using Astrum.Projects.Aggregates;

namespace Astrum.Projects.Specifications.Customer;

public class GetCustomFieldsSpec : Specification<CustomField>
{
}

public class GetCustomFieldByIdSpec : GetCustomFieldsSpec
{
    public GetCustomFieldByIdSpec(Guid id)
    {
        Query
            .Where(x => x.Id == id);
    }
}