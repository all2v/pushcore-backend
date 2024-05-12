using Astrum.CodeRev.Application.TrackerService.ViewModels.DTO;
using Astrum.SharedLib.Common.Results;

namespace Astrum.CodeRev.Application.TrackerService.Services;

public interface ITrackerService
{
    public Task<Result<List<RecordChunkDto>>> Get(Guid taskSolutionId, decimal? saveTime);
    public Task<Result<LastCodeDto?>> GetLastCode(Guid taskSolutionId);
    public Task<Result> Create(TaskRecordDto request);
}