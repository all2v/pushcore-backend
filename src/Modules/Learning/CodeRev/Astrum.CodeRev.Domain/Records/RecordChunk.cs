using System.Runtime.Serialization;

namespace Astrum.CodeRev.Domain.Records;

[DataContract]
public class RecordChunk
{
    [DataMember] public decimal SaveTime { get; set; }
    [DataMember] public string Code { get; set; }
    [DataMember] public List<Record> Records { get; set; }
}