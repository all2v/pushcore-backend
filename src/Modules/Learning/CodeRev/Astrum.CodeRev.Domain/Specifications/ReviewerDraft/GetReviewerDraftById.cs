using Ardalis.Specification;

namespace Astrum.CodeRev.Domain.Specifications.ReviewerDraft;

public class GetReviewerDraftById : Specification<CodeRev.Domain.Aggregates.ReviewerDraft>
{
    public GetReviewerDraftById(Guid draftId)
    {
        Query.Where(draft => draft.Id == draftId);
    }
}