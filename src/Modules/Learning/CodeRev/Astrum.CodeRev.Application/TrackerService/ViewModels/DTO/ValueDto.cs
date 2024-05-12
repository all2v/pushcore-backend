using System.Runtime.Serialization;

namespace Astrum.CodeRev.Application.TrackerService.ViewModels.DTO;

[DataContract]
public class ValueDto
{
    [DataMember]
    public string[] Value { get; set; }
}