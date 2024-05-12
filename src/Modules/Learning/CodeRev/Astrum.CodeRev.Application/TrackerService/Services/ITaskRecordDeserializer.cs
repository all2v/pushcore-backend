using Astrum.CodeRev.Application.TrackerService.ViewModels.DTO;

namespace Astrum.CodeRev.Application.TrackerService.Services;

public interface ITaskRecordDeserializer
{
    public TaskRecordDto ParseRequestDto(TaskRecordRequestDto request);
}