using Astrum.Account.Services;
using Astrum.SharedLib.Common.CQS.Implementations;
using Astrum.SharedLib.Common.Results;

namespace Astrum.Account.Features.Achievement.Queries.GetAchievement;

public class GetAchievementQueryHandler : QueryResultHandler<GetAchievementQuery, AchievementResponse>
{
    private readonly IAchievementService _achievementService;

    public GetAchievementQueryHandler(IAchievementService achievementService)
    {
        _achievementService = achievementService;
    }

    public override async Task<Result<AchievementResponse>> Handle(GetAchievementQuery query,
        CancellationToken cancellationToken = default)
    {
        var response = await _achievementService.GetAsync(query.Id);
        return response;
    }
}