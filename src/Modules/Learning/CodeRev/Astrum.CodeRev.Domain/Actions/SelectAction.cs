using System.Runtime.Serialization;

namespace Astrum.CodeRev.Domain.Actions;

[DataContract]
public class SelectAction
{
    [DataMember] public int LineNumber { get; set; }

    [DataMember] public List<MoveAction> TailMove { get; set; }
}