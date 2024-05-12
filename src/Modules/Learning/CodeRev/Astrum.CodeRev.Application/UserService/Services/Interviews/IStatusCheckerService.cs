
using Astrum.CodeRev.Domain.Aggregates.Enums;

namespace Astrum.CodeRev.Application.UserService.Services.Interviews;

public interface IStatusCheckerService
{
    bool IsSolutionTimeExpired(long endTimeMs);
    bool HasReviewerCheckResult(Grade grade);
    bool HasHrCheckResult(InterviewResult interviewResult);
}