using System.Runtime.Serialization;

namespace Astrum.CodeRev.Application.TrackerService.ViewModels.DTO;

[DataContract]
public class PeriodDto
{
    [DataMember] public IndexDto From { get; set; }
    [DataMember] public IndexDto? To { get; set; }
}