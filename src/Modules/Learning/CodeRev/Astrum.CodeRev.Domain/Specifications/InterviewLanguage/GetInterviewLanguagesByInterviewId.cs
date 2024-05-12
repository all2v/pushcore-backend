
using Ardalis.Specification;

namespace Astrum.CodeRev.Domain.Specifications.InterviewLanguage;

public class GetInterviewLanguagesByInterviewId :Specification<Aggregates.InterviewLanguage>
{
    public GetInterviewLanguagesByInterviewId(Guid interviewId)
    {
        Query
            .Where(language => language.InterviewId == interviewId);
    }
}