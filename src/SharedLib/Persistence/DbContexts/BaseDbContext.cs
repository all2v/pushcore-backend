using System.Data;
using System.Linq.Expressions;
using Astrum.SharedLib.Application.Contracts.Infrastructure;
using Astrum.SharedLib.Domain.Interfaces;
using Astrum.SharedLib.Persistence.Extensions;
using Astrum.SharedLib.Persistence.Models.Audit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Astrum.SharedLib.Persistence.DbContexts;

/// <summary>
/// </summary>
public abstract class BaseDbContext : DbContext, IDbContext
{
    private IDbContextTransaction _transaction;

    /// <summary>
    /// </summary>
    /// <param name="options"></param>
    public BaseDbContext(DbContextOptions options) : base(options)
    {
    }

    /// <summary>
    /// </summary>
    public DbSet<AuditHistory> AuditHistory { get; set; }

    #region IDbContext Members

    /// <summary>
    ///     Overrides the <see cref="SaveChanges(bool)" />
    /// </summary>
    /// <param name="acceptAllChangesOnSuccess"></param>
    /// <returns></returns>
    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        this.CustomSaveChangesAsync().Wait();

        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    /// <summary>
    ///     Overrides the <see cref="DbContext.SaveChanges()" />.
    /// </summary>
    /// <returns></returns>
    public override int SaveChanges()
    {
        this.CustomSaveChangesAsync().Wait();

        return base.SaveChanges(true);
    }

    /// <summary>
    ///     Overrides the <see cref="DbContext.SaveChangesAsync(bool, CancellationToken)" />
    /// </summary>
    /// <param name="acceptAllChangesOnSuccess"></param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns></returns>
    public async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = default, bool ensureAudit = true)
    {
        await this.CustomSaveChangesAsync(cancellationToken, ensureAudit);

        return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    /// <summary>
    ///     Overrides the <see cref="SaveChangesAsync(CancellationToken)" />
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns></returns>
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default, bool ensureAudit = true)
    {
        await this.CustomSaveChangesAsync(cancellationToken, ensureAudit);
        return await base.SaveChangesAsync(true, cancellationToken);
    }

    /// <summary>
    /// </summary>
    public void BeginTransaction()
    {
        _transaction = Database.BeginTransaction();
    }

    // TODO isn't synchronise
    /// <summary>
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        _transaction = await Database.BeginTransactionAsync(cancellationToken);
    }

    /// <summary>
    /// </summary>
    /// <param name="isolationLevel"></param>
    /// <param name="cancellationToken">Cancellation token</param>
    public async Task BeginTransactionAsync(IsolationLevel isolationLevel,
        CancellationToken cancellationToken = default)
    {
        _transaction = await Database.BeginTransactionAsync(isolationLevel, cancellationToken);
    }

    /// <summary>
    /// </summary>
    public void Commit()
    {
        try
        {
            SaveChanges();
            _transaction.Commit();
        }
        finally
        {
            _transaction.Dispose();
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await SaveChangesAsync(cancellationToken);
            await _transaction.CommitAsync(cancellationToken);
        }
        finally
        {
            _transaction.Dispose();
        }
    }

    /// <summary>
    /// </summary>
    public void Rollback()
    {
        _transaction.Rollback();
        _transaction.Dispose();
    }

    #endregion

    /// <summary>
    ///     Entity model definitions
    /// </summary>
    /// <param name="builder"></param>
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.EnableAuditHistory();
        var schema = GetType().Name.Replace("DbContext", "");
        builder.HasDefaultSchema(schema);

        // ExtractExternalEntities(builder);
        builder.ApplyConfigurationsFromAssembly(GetType().Assembly);

        foreach (var mutableEntityType in  builder.Model.GetEntityTypes())
        {
            if (mutableEntityType.ClrType.IsAssignableTo(typeof(IAuditableEntity)))
            {
                builder.Entity(mutableEntityType.ClrType)
                    .Property(nameof(IAuditableEntity.DateDeleted))
                    .IsRequired(false);
            }
            if (mutableEntityType.ClrType.IsAssignableTo(typeof(ISafetyRemovableEntity)))
            {

                // Create the query filter
                var parameter = Expression.Parameter(mutableEntityType.ClrType);

                // EF.Property<bool>(post, "IsDeleted")
                var propertyMethodInfo = typeof(EF).GetMethod("Property").MakeGenericMethod(typeof(bool));
                var isDeletedProperty = Expression.Call(propertyMethodInfo, parameter, Expression.Constant("IsDeleted"));

                // EF.Property<bool>(post, "IsDeleted") == false
                BinaryExpression compareExpression = Expression.MakeBinary(ExpressionType.Equal, isDeletedProperty, Expression.Constant(false));

                // post => EF.Property<bool>(post, "IsDeleted") == false
                var lambda = Expression.Lambda(compareExpression, parameter);
                builder.Entity(mutableEntityType.ClrType).HasQueryFilter(lambda);
            }
        }
    }

    private void ExtractExternalEntities(ModelBuilder builder)
    {
        var currentAssembly = GetType().Assembly;
        var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();
        var referencedAssembliesNames = currentAssembly.GetReferencedAssemblies();
        var referencedAssemblies = loadedAssemblies
            // .Where(a => referencedAssembliesNames.Any(x => x.ToString() == a.FullName))
            .Where(a => a.Location!.Contains("Modules"))
            .Where(a => a.ManifestModule.Name.EndsWith("Persistence"))
            //          ||   a.FullName.Contains("Infrastructure.Models")
            // )
            .ToList();
        referencedAssemblies.Add(currentAssembly);

        var sharedPersistenceAssembly = typeof(AuditHistory).Assembly;

        bool IsIEntityTypeConfiguration(Type i)
        {
            return i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>);
        }

        var entityTypeConfigurations = referencedAssemblies.SelectMany(a => a.GetTypes())
            .Where(t => t.GetInterfaces().Any(IsIEntityTypeConfiguration));
        var entityTypeConfigurationsTypes = entityTypeConfigurations
            .Select(t => t.GetInterfaces().First(IsIEntityTypeConfiguration))
            .Select(i => i.GetGenericArguments().First()).ToList();
        var outsideEntityTypes = entityTypeConfigurationsTypes
            .Where(e => e.Assembly != currentAssembly && e.Assembly != sharedPersistenceAssembly)
            .ToList();

        foreach (var referencedAssembly in referencedAssemblies)
            builder.ApplyConfigurationsFromAssembly(referencedAssembly);

        foreach (var outsideEntityType in outsideEntityTypes)
            builder.Entity(outsideEntityType)
                .ToTable(x => x.ExcludeFromMigrations());
    }

    private List<Type> GetAllDependentTypes(Type type, ModelBuilder builder, List<Type>? checkedTyped = null)
    {
        checkedTyped ??= new List<Type>();
        if (!checkedTyped.Any(t => t == type))
            checkedTyped.Add(type);
        var userEntityType = builder.Model.FindEntityType(type);
        var types = userEntityType.GetAllBaseTypes().ToList();
        var navigationProps = userEntityType
            .GetNavigations();

        foreach (var navigationProp in navigationProps)
        {
            var navigationPropertyType = navigationProp.IsCollection
                ? navigationProp.ClrType.GetGenericArguments().First()
                : navigationProp.ClrType;

            if (checkedTyped.All(x => x != navigationPropertyType))
                GetAllDependentTypes(navigationPropertyType, builder, checkedTyped);
        }

        return checkedTyped;
    }
}