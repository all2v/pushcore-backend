using System.Runtime.Serialization;
using Astrum.CodeRev.Domain.Actions;
using Astrum.CodeRev.Domain.Primitives;

namespace Astrum.CodeRev.Domain.Records;

[DataContract]
public class Operation
{
    /// <summary>
    ///     o
    /// </summary>
    [DataMember]
    public OperationTypePrimitive Type { get; set; }

    /// <summary>
    ///     i
    /// </summary>
    [DataMember]
    public PeriodPrimitive Index { get; set; }

    /// <summary>
    ///     a
    /// </summary>
    [DataMember]
    public ValuePrimitive? Value { get; set; }

    /// <summary>
    ///     r
    /// </summary>
    [DataMember]
    public List<RemoveAction>? Remove { get; set; }

    /// <summary>
    ///     s
    /// </summary>
    [DataMember]
    public List<SelectAction>? Select { get; set; }

    /// <summary>
    ///     e
    /// </summary>
    [DataMember]
    public string? Extra { get; set; }

    /// <summary>
    ///     d
    /// </summary>
    [DataMember]
    public string? Delete { get; set; }
}