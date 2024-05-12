using System.Runtime.Serialization;
using MongoDB.Bson;

namespace Astrum.CodeRev.Application.TrackerService.ViewModels.DTO;

[DataContract]
public class TaskRecordDto
{
    [DataMember] public ObjectId Id { get; set; }

    [DataMember] public Guid TaskSolutionId { get; set; }

    [DataMember] public string Code { get; set; }

    [DataMember] public List<RecordChunkDto> RecordChunks { get; set; }
}