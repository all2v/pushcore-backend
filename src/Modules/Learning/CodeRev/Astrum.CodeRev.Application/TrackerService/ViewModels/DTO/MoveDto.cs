using System.Runtime.Serialization;

namespace Astrum.CodeRev.Application.TrackerService.ViewModels.DTO;

[DataContract]
public class MoveDto
{
    [DataMember] public int Start { get; set; }

    [DataMember] public int? End { get; set; }
}