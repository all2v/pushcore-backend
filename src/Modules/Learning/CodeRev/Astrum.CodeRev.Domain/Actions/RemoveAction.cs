using System.Runtime.Serialization;

namespace Astrum.CodeRev.Domain.Actions;

[DataContract]
public class RemoveAction
{
    [DataMember] public int Long { get; set; }

    [DataMember] public int Count { get; set; }
}