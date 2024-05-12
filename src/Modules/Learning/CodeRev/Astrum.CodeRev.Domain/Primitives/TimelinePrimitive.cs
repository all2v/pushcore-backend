using System.Runtime.Serialization;

namespace Astrum.CodeRev.Domain.Primitives;

[DataContract]
public class TimelinePrimitive
{
    [DataMember]
    public int Start { get; set; }

    [DataMember]
    public int? End { get; set; }
}