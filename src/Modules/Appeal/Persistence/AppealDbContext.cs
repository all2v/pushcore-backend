using Astrum.Appeal.Aggregates;
using Astrum.Appeal.Domain.Aggregates;
using Astrum.SharedLib.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Astrum.Appeal;

public class AppealDbContext : BaseDbContext
{
    public AppealDbContext(DbContextOptions<AppealDbContext> options) : base(options)
    {
    }

    public DbSet<Aggregates.Appeal> Appeals { get; set; }
    public DbSet<AppealCategory> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.HasDefaultSchema("Appeal");
        builder
            .Entity<AppealAppealCategory>()
            .HasOne(app => app.Category)
            .WithMany(cat => cat.Appeals)
            .HasForeignKey(bc => bc.AppealCategoryId);
        builder
            .Entity<AppealAppealCategory>()
            .HasOne(app => app.Appeal)
            .WithMany(cat => cat.AppealCategories)
            .HasForeignKey(bc => bc.AppealId);
    }
}