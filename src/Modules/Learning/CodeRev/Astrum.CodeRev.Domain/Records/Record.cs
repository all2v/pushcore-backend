using System.Runtime.Serialization;
using Astrum.CodeRev.Domain.Primitives;

namespace Astrum.CodeRev.Domain.Records;

[DataContract]
public class Record
{
    [DataMember] public TimelinePrimitive Time { get; set; }

    [DataMember] public int? Long { get; set; }

    [DataMember] public List<Operation> Operation { get; set; }
}