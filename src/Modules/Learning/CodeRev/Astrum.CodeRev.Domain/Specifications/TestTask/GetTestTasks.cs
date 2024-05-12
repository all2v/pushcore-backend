using Ardalis.Specification;

namespace Astrum.CodeRev.Domain.Specifications.TestTask;

public class GetTestTasks:Specification<Aggregates.TestTask>
{
    public GetTestTasks(int offset,int limit)
    {
        Query.Skip(offset).Take(limit);
    }
}