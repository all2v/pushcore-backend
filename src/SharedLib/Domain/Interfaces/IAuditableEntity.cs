namespace Astrum.SharedLib.Domain.Interfaces;

/// <summary>
///     Base auditable entity class to be inherited from entities which should be audited
/// </summary>
public interface IAuditableEntity
{
    /// <summary>
    ///     The date the entity was created
    /// </summary>
    public DateTimeOffset DateCreated { get; set; }

    /// <summary>
    ///     The date the entity was last modified
    /// </summary>
    public DateTimeOffset DateModified { get; set; }

    /// <summary>
    ///     The date the entity was last modified
    /// </summary>
    public DateTimeOffset? DateDeleted { get; set; }

    /// <summary>
    ///     The user who created the entity
    /// </summary>
    public string? CreatedBy { get; set; }

    /// <summary>
    ///     The user who last modified the entity
    /// </summary>
    public string? ModifiedBy { get; set; }
}

public interface IAuditableEntity<out TId> : IAuditableEntity, IBaseEntity<TId>
{
}