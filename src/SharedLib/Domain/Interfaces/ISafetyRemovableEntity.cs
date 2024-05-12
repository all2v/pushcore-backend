namespace Astrum.SharedLib.Domain.Interfaces;

/// <summary>
///     Interface indicates that the record in database is not deleting, but the entity marks as deleted
/// </summary>
public interface ISafetyRemovableEntity
{
    /// <summary>
    ///     Property indicating that the entity has been deleted
    /// </summary>
    bool IsDeleted { get; }

    void MarkAsDeleted();
}