using Astrum.SharedLib.Domain.Interfaces;

namespace Astrum.SharedLib.Domain.Entities;

/// <inheritdoc cref="IAuditableEntity" />
/// <inheritdoc cref="BaseEntity{TId}" />
public abstract class AuditableEntity<TId> : BaseEntity<TId>, IAuditableEntity<TId>
{
    #region IAuditableEntity<TId> Members

    public DateTimeOffset DateCreated { get; set; } = DateTimeOffset.UtcNow;

    public DateTimeOffset DateModified { get; set; } = DateTimeOffset.UtcNow;

    public DateTimeOffset? DateDeleted { get; set; }

    public string? CreatedBy { get; set; }
    public string? ModifiedBy { get; set; }

    #endregion
}