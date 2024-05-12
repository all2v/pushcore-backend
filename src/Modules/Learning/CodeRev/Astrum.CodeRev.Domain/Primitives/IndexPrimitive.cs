using System.Runtime.Serialization;

namespace Astrum.CodeRev.Domain.Primitives;

[DataContract]
public class IndexPrimitive
{
    [DataMember]
    public int LineNumber { get; set; }

    [DataMember]
    public int ColumnNumber { get; set; }
}