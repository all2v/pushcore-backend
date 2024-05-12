using Astrum.CodeRev.Application.TrackerService.ViewModels.DTO;

namespace Astrum.CodeRev.Application.TrackerService.Services;

public interface ITaskRecordSerializer
{
    public List<RecordChunkResponseDto> Serialize(List<RecordChunkDto>? recordChunks);
}