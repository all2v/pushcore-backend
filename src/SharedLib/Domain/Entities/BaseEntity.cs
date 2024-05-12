using Astrum.SharedLib.Domain.Interfaces;

namespace Astrum.SharedLib.Domain.Entities;

public abstract class BaseEntity<TId> : IBaseEntity<TId>
{
    #region IBaseEntity<TId> Members

    /// <summary>
    ///     The aggregate root Id
    /// </summary>
    /// TODO! to protected setter
    public TId? Id { get; set; }

    /// <summary>
    ///     Indicates whether this aggregate is logically deleted
    /// </summary>
    public bool IsDeleted { get; set; }

    #endregion

    public void MarkAsDeleted()
    {
        IsDeleted = true;
    }
}