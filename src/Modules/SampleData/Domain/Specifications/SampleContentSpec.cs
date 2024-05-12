using Ardalis.Specification;
using Astrum.SampleData.Aggregates;

namespace Astrum.SampleData.Specifications;

public class GetSampleContentByIdSpec : Specification<SampleContentFile>
{
    public GetSampleContentByIdSpec(Guid id)
    {
        Query
            .Where(e => e.Id == id);
    }
}

public class SampleContentOrderByNameSpec : Specification<SampleContentFile>
{
    public SampleContentOrderByNameSpec()
    {
        Query
            .OrderBy(e => e.ContextName);
    }
}
