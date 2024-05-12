using System.Runtime.CompilerServices;
using Astrum.Identity.Domain.Entities;
using Astrum.Identity.Models;
using Astrum.SharedLib.Application.Contracts.Persistence.Repositories;
using Astrum.SharedLib.Persistence.Extensions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

[assembly: InternalsVisibleTo("Astrum.Tests")]

namespace Astrum.Identity;

public class IdentityDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid, ApplicationUserClaim,
        ApplicationUserRole, ApplicationUserLogin, ApplicationRoleClaim, ApplicationUserToken>,
    IUnitOfWork
{
    public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options)
    {
    }

    //To add migration with pmc:
    //Add-Migration Migration -Context IdentityDbContext
    //To update db:
    //Update-Database -Context IdentityDbContext
    public DbSet<GitLabUsersMappings> GitlabMappings { get; set; }
    public DbSet<GitlabUser> GitlabUsers { get; set; }

    #region IUnitOfWork Members

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
    /// <param name="cancellationToken"></param>
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
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default, bool ensureAudit = true)
    {
        await this.CustomSaveChangesAsync(cancellationToken, ensureAudit);
        return await base.SaveChangesAsync(true, cancellationToken);
    }

    #endregion

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        var schema = GetType().Name.Replace("DbContext", "");
        builder.HasDefaultSchema(schema);
        var currentAssembly = GetType().Assembly;
        builder.ApplyConfigurationsFromAssembly(currentAssembly);
        builder.EnableAuditHistory();
    }
}
