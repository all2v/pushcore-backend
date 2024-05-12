using System.Runtime.Serialization;

namespace Astrum.CodeRev.Application.TrackerService.ViewModels.DTO;

[DataContract]
public class RecordChunkDto
{
    [DataMember] public decimal SaveTime { get; set; }
    [DataMember] public string Code { get; set; }
    [DataMember] public List<RecordDto> Records { get; set; }
}