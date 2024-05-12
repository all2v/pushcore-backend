using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;

namespace Astrum.CodeRev.Application.TrackerService.ViewModels.DTO;

[DataContract]
public class TaskRecordRequestDto
{
    [DataMember] public Guid TaskSolutionId { get; set; }
    [DataMember] public string Code { get; set; }
    [DataMember] public decimal SaveTime { get; set; }
    [DataMember] public List<JObject> Records { get; set; }
}