using System.Runtime.Serialization;

namespace Astrum.CodeRev.Application.TrackerService.ViewModels.DTO;

[DataContract]
public class LastCodeDto
{
    [DataMember] public string? Code { get; set; }
}