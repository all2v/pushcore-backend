using Astrum.ITDictionary.Enums;
using Astrum.ITDictionary.Models.ViewModels;
using Astrum.SharedLib.Common.Results;

namespace Astrum.ITDictionary.Services;

public interface IStatisticsService
{
    public Task<Result<StatisticsSummary>> GetSummary(Guid userId);

    public Task<Result<TestStatisticsDetails>> GetTestStatsWithDetails(Guid userId, PracticeType type);
}