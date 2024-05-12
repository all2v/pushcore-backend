
using Astrum.CodeRev.Domain.Aggregates.Enums;

namespace Astrum.CodeRev.Application.UserService.Services.Interviews;

public class StatusCheckerService : IStatusCheckerService
{
    public bool IsSolutionTimeExpired(long endTimeMs)
    {
        var nowTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        return endTimeMs < nowTime;
    }

    public bool HasReviewerCheckResult(Grade grade) => grade != Grade.Zero;

    public bool HasHrCheckResult(InterviewResult interviewResult) => interviewResult != InterviewResult.NotChecked;
}