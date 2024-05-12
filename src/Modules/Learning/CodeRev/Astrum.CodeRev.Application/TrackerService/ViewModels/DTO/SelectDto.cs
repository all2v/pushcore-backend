using System.Runtime.Serialization;

namespace Astrum.CodeRev.Application.TrackerService.ViewModels.DTO;


[DataContract]
public class SelectDto
{
    [DataMember] public int LineNumber { get; set; }

    [DataMember] public List<MoveDto> TailMove { get; set; }
}