using Astrum.SharedLib.Domain.Entities;
using Astrum.SharedLib.Domain.Interfaces;

namespace Astrum.SharedLib.Persistence.Models;

/// <summary>
///     Base data entity class to be inherited from every entity in database context
///     Inherits <see cref="AuditableEntity{TId}" /> for convenience.
/// </summary>
public class DataEntityBase<TId> : AuditableEntity<TId>, IDataEntity<TId>
{
    #region IDataEntity<TId> Members

    public TId Id { get; set; }

    #endregion
}