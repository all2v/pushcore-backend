using Ardalis.Specification;
using Astrum.CodeRev.Domain.Aggregates;

namespace Astrum.CodeRev.Domain.Specifications.Interviews;

public class GetAllVacancies : Specification<Interview>
{
    public GetAllVacancies()
    {
        Query
            .OrderBy(interview => interview.Vacancy);
    }
    
}