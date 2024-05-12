using System.Runtime.Serialization;

namespace Astrum.CodeRev.Application.TrackerService.ViewModels.DTO;

[DataContract]
public class RemoveDto
{
    [DataMember] public int Long { get; set; }

    [DataMember] public int Count { get; set; }
}