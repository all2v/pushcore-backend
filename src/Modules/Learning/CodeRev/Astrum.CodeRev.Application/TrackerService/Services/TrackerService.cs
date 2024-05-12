using Astrum.CodeRev.Application.TrackerService.ViewModels.DTO;
using Astrum.CodeRev.Domain.Records;
using Astrum.CodeRev.Domain.Repositories;
using Astrum.SharedLib.Common.Results;
using AutoMapper;

namespace Astrum.CodeRev.Application.TrackerService.Services;

public class TrackerService : ITrackerService
{
    private readonly ITaskRecordRepository _recordRepository;
    private readonly IMapper _mapper;

    public TrackerService(ITaskRecordRepository recordRepository, IMapper mapper)
    {
        this._recordRepository = recordRepository;
        this._mapper = mapper;
    }

    public async Task<Result<List<RecordChunkDto>>> Get(Guid taskSolutionId, decimal? saveTime)
    {
        var recordsRequest = await _recordRepository.Get(taskSolutionId);
        if (recordsRequest == null)
            return Result<List<RecordChunkDto>>.Error($"Не существует записей у решения с id {taskSolutionId}");
        var result = recordsRequest.RecordChunks.Where(x => x.SaveTime > (saveTime ?? 0m))
            .OrderBy(x => x.SaveTime)
            .ToList();

        return Result<List<RecordChunkDto>>.Success(result.Select(chunk => _mapper.Map<RecordChunkDto>(chunk))
            .ToList());
    }

    public async Task<Result<LastCodeDto?>> GetLastCode(Guid taskSolutionId)
    {
        var taskRecord = await _recordRepository.Get(taskSolutionId);
        return Result<LastCodeDto?>.Success(new LastCodeDto { Code = taskRecord?.Code });
    }

    public async Task<Result> Create(TaskRecordDto request)
    {
        await _recordRepository.Save(_mapper.Map<TaskRecord>(request));
        return Result.Success();
    }
}