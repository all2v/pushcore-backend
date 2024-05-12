using System.Runtime.Serialization;

namespace Astrum.CodeRev.Application.TrackerService.ViewModels.DTO;

[DataContract]
public class RecordDto
{
    [DataMember] public TimelineDto Time { get; set; }

    [DataMember] public int? Long { get; set; }

    [DataMember] public List<OperationDto> Operation { get; set; }
}