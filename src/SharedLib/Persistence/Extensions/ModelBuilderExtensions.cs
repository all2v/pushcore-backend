using System.Reflection;
using Astrum.SharedLib.Persistence.Configurations;
using Astrum.SharedLib.Persistence.ModelBuilders;
using Microsoft.EntityFrameworkCore;

namespace Astrum.SharedLib.Persistence.Extensions;

/// <summary>
/// </summary>
public static class ModelBuilderExtensions
{
    /// <summary>
    ///     Enables auditing change history.
    /// </summary>
    /// <param name="modelBuilder">The <see cref="ModelBuilder" /> to enable auto history functionality.</param>
    /// <returns>The <see cref="ModelBuilder" /> to enable auto history functionality.</returns>
    public static ModelBuilder EnableAuditHistory(this ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AuditHistoryEntityTypeConfiguration());

        return modelBuilder;
    }

    public static ModelBuilder BuildEntities(this ModelBuilder builder, Assembly assembly)
    {
        var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();
        var referencedAssembliesNames = assembly.GetReferencedAssemblies();
        var referencedAssemblies = loadedAssemblies
            .Where(a => referencedAssembliesNames.Any(x => x.ToString() == a.FullName))
            .ToList();
        referencedAssemblies.Add(assembly);

        var customModelBuilderTypes = referencedAssemblies
            .SelectMany(a => a.GetTypes())
            .Where(t => typeof(ICustomModelBuilder).IsAssignableFrom(t) && !t.IsAbstract)
            .Select(t => (ICustomModelBuilder?)Activator.CreateInstance(t))
            .ToList();

        foreach (var customModelBuilder in customModelBuilderTypes)
            customModelBuilder?.Build(builder);

        return builder;
    }
}