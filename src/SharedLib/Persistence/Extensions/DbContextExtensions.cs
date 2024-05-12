using System.Text.Json;
using Astrum.Identity.Contracts;
using Astrum.SharedLib.Persistence.Helpers;
using Astrum.SharedLib.Persistence.Models.Audit;
using Astrum.SharedLib.Persistence.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Astrum.SharedLib.Persistence.Extensions;

public static class DbContextExtensions
{
    public static string BaseConnectionName = "BaseConnection";

    /// <summary>
    ///     Contains additional functionality when SaveChanges happens
    /// </summary>
    /// <param name="context"></param>
    /// <param name="cancellationToken"></param>
    public static async Task CustomSaveChangesAsync(this DbContext context,
        CancellationToken cancellationToken = default, bool ensureAudit = true)
    {
        var authenticatedUser = context.GetService<IAuthenticatedUserService>();
        DbContextStateOperations.Change(context.ChangeTracker, authenticatedUser.Username);

        if(ensureAudit)
            context.EnsureAuditHistory(authenticatedUser.Username);

        var mediatr = context.GetService<IMediator>();
        await mediatr.DispatchDomainEventsAsync<Guid>(context.ChangeTracker, cancellationToken);
    }

    /// <summary>
    ///     Ensures the automatic auditing history.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="username">User made the action.</param>
    public static void EnsureAuditHistory(this DbContext context, string username)
    {
        var entries = context.ChangeTracker.Entries()
            .Where(e => !AuditUtilities.IsAuditDisabled(e.Entity.GetType()) &&
                        e.State is EntityState.Added or EntityState.Modified or EntityState.Deleted)
            .ToArray();
        foreach (var entry in entries) context.Add((object)entry.AutoHistory(username));
    }

    private static AuditHistory AutoHistory(this EntityEntry entry, string username)
    {
        var history = new AuditHistory
        {
            TableName = entry.Metadata.GetTableName()!,
            Username = username
        };

        // Get the mapped properties for the entity type.
        // (include shadow properties, not include navigations & references)
        var properties = entry.Properties
            .Where(p => !AuditUtilities.IsAuditDisabled(p.EntityEntry.Entity.GetType(), p.Metadata.Name));

        // TODO костыль
        if (properties.All(prop => prop.Metadata.IsPrimaryKey()))
            history.RowId = "0";

        foreach (var prop in properties)
        {
            var propertyName = prop.Metadata.Name;
            if (prop.Metadata.IsPrimaryKey())
            {
                history.AutoHistoryDetails.NewValues[propertyName] = prop.CurrentValue;
                continue;
            }

            switch (entry.State)
            {
                case EntityState.Added:
                    history.RowId = "0";
                    history.Kind = EntityState.Added;
                    history.AutoHistoryDetails.NewValues.Add(propertyName, prop.CurrentValue);
                    break;

                case EntityState.Modified:
                    history.RowId = entry.PrimaryKey();
                    history.Kind = EntityState.Modified;
                    history.AutoHistoryDetails.OldValues.Add(propertyName, prop.OriginalValue);
                    history.AutoHistoryDetails.NewValues.Add(propertyName, prop.CurrentValue);
                    break;

                case EntityState.Deleted:
                    history.RowId = entry.PrimaryKey();
                    history.Kind = EntityState.Deleted;
                    history.AutoHistoryDetails.OldValues.Add(propertyName, prop.OriginalValue);
                    break;
            }
        }

        history.Changed = JsonSerializer.Serialize(history.AutoHistoryDetails);

        return history;
    }

    private static string PrimaryKey(this EntityEntry entry)
    {
        var key = entry.Metadata.FindPrimaryKey();

        var values = new List<object>();
        foreach (var property in key.Properties)
        {
            var value = entry.Property(property.Name).CurrentValue;
            if (value != null) values.Add(value);
        }

        return string.Join(",", values);
    }
}