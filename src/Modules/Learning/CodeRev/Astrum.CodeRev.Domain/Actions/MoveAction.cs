using System.Runtime.Serialization;

namespace Astrum.CodeRev.Domain.Actions;

[DataContract]
public class MoveAction
{
    [DataMember] public int Start { get; set; }

    [DataMember] public int? End { get; set; }
}