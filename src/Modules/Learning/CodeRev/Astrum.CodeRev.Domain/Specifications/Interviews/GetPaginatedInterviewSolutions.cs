using Ardalis.Specification;

namespace Astrum.CodeRev.Domain.Specifications.Interviews;

public class GetPaginatedInterviewSolutions : GetAsynchronousSubmittedInterviewSolutions
{
    public GetPaginatedInterviewSolutions(int offset, int limit)
    {
        Query.Skip(offset).Take(limit);
    }
}