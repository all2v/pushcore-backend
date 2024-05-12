using Astrum.SharedLib.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Astrum.SharedLib.Persistence.Utilities;

/// <summary>
///     Contains a set of automatic operations against Database
/// </summary>
public static class DbContextStateOperations
{
    public static void Change(ChangeTracker changeTracker, string user)
    {
        var now = DateTimeOffset.UtcNow;
        
        foreach (var change in changeTracker.Entries())
        {
            if (change.Entity is IAuditableEntity auditableEntity)
            {
                if (change.State == EntityState.Added)
                {
                    auditableEntity.DateCreated = now;
                    if (string.IsNullOrWhiteSpace(auditableEntity.CreatedBy))
                        auditableEntity.CreatedBy = user;
                }

                auditableEntity.DateModified = now;
                auditableEntity.ModifiedBy = user;
            
                if(change is {Entity: ISafetyRemovableEntity, State: EntityState.Deleted})
                    auditableEntity.DateDeleted = now;
            }
            if (change is {Entity: ISafetyRemovableEntity safetyRemovableEntity, State: EntityState.Deleted})
            {
                safetyRemovableEntity.MarkAsDeleted();
                change.State = EntityState.Modified;
            }

            if (change is { Entity: ISafetyRemovableOwnEntity , State: EntityState.Deleted })
            {
                change.State = EntityState.Unchanged;
            }
        }
    }

    /// <summary>
    ///     Automatically updates the BaseEntity properties without any developer action
    /// </summary>
    /// <param name="changes">The collection of BaseEntity entities for save to DB</param>
    /// <param name="username">The logged in user</param>
    /// <param name="changeTime"></param>
    // public static void UpdateAuditableProperties(IEnumerable<EntityEntry<IAuditableEntity>> changes, string username, DateTimeOffset? changeTime = null)
    // {
    //     changeTime ??= DateTimeOffset.UtcNow;
    //     foreach (var change in changes)
    //     {
    //         if (change.State == EntityState.Added)
    //         {
    //             change.Entity.DateCreated = changeTime.Value;
    //             change.Entity.CreatedBy = username;
    //         }
    //
    //         change.Entity.DateModified = changeTime.Value;
    //         change.Entity.ModifiedBy = username;
    //         
    //         
    //         if(change.Entity is ISafetyRemovableEntity)
    //             change.Entity.DateDeleted = changeTime.Value;
    //     }
    // }
    //
    // public static void SafetyDeleteEntity(IEnumerable<EntityEntry<ISafetyRemovableEntity>> changes)
    // {
    //     foreach (var change in changes)
    //         if (change.State == EntityState.Deleted)
    //         {
    //             change.Entity.MarkAsDeleted();
    //             change.State = EntityState.Modified;
    //         }
    // }
}