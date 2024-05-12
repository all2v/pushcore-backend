using Astrum.CodeRev.Domain.Aggregates;
using Astrum.CodeRev.Domain.Aggregates.Draft;
using Astrum.CodeRev.Domain.Repositories;
using Astrum.CodeRev.Domain.Specifications.ReviewerDraft;
using Astrum.SharedLib.Common.Results;

namespace Astrum.CodeRev.Application.UserService.Services;

public class DraftService : IDraftService
{
    private readonly IReviewerDraftRepository _reviewerDraftRepository;

    public DraftService(IReviewerDraftRepository reviewerDraftRepository)
    {
        this._reviewerDraftRepository = reviewerDraftRepository;
    }

    private async Task<ReviewerDraft?> GetReviewerDraft(Guid draftId)
    {
        return await _reviewerDraftRepository.FirstOrDefaultAsync(new GetReviewerDraftById(draftId));
    }

    public async Task<Result<Draft>> GetDraft(Guid draftId)
    {
        var draft = await GetReviewerDraft(draftId);
        return draft == null
            ? Result<Draft>.Error($"Заметки с id {draftId} не существует")
            : Result<Draft>.Success(draft.Draft);
    }

    public async Task<Result> PutDraft(Guid draftId, Draft draft)
    {
        var reviewerDraft = await GetReviewerDraft(draftId);
        if (reviewerDraft == null)
            return Result.Error($"Заметки с id {draftId} не существует");
        reviewerDraft.Draft = draft;
        await _reviewerDraftRepository.UnitOfWork.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result<ReviewerDraft>> Create(Guid interviewSolutionId)
    {
        var reviewerDraftId = Guid.NewGuid();
        var draft = new ReviewerDraft
        {
            Id = reviewerDraftId,
            InterviewSolutionId = interviewSolutionId,
            Draft = null
        };
        await _reviewerDraftRepository.AddAsync(draft);
        await _reviewerDraftRepository.UnitOfWork.SaveChangesAsync();
        return Result<ReviewerDraft>.Success(draft);
    }
}