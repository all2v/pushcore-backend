using Astrum.CodeRev.Domain.Aggregates;
using Astrum.CodeRev.Domain.Aggregates.Draft;
using Astrum.SharedLib.Common.Results;

namespace Astrum.CodeRev.Application.UserService.Services;

public interface IDraftService
{
    Task<Result<Draft>> GetDraft(Guid draftId);
    Task<Result> PutDraft(Guid draftId, Draft draft);
    
    Task<Result<ReviewerDraft>> Create(Guid interviewSolutionId);
}