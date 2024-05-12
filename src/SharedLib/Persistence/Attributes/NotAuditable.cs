namespace Astrum.SharedLib.Persistence.Attributes;

/// <summary>
///     Custom data annotation to handle if a class(table) or property(field) is not auditable.By default is true.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, Inherited = false)]
public class NotAuditableAttribute : Attribute
{
    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="nonAuditable">Is NOT auditable</param>
    public NotAuditableAttribute(bool nonAuditable = true)
    {
        Enabled = nonAuditable;
    }

    /// <summary>
    ///     Defines if class(table) or property(field) is auditable or not
    /// </summary>
    public bool Enabled { get; set; }
}