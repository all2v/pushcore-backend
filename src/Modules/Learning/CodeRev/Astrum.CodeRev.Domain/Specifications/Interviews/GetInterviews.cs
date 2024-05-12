using Ardalis.Specification;
using Astrum.CodeRev.Domain.Aggregates;

namespace Astrum.CodeRev.Domain.Specifications.Interviews;

public class GetInterviews : Specification<Interview>
{
    public GetInterviews(int offset, int limit)
    {
        Query.Skip(offset).Take(limit);
    }
}