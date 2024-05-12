using System.Runtime.Serialization;

namespace Astrum.CodeRev.Domain.Primitives;

[DataContract]
public class ValuePrimitive
{
    [DataMember]
    public List<string> Value { get; set; }
}