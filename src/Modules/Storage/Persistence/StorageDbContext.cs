using Astrum.SharedLib.Persistence.DbContexts;
using Astrum.Storage.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace Astrum.Storage;

public class StorageDbContext : BaseDbContext
{
    public StorageDbContext(DbContextOptions<StorageDbContext> options) : base(options)
    {
    }

    //To add migration with pmc:
    //Add-Migration Migration -Context StorageDbContext -Output Data/Migrations/Storage
    //To update db:
    //Update-Database -Context StorageDbContext
    public DbSet<StorageFile> Files { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.HasDefaultSchema("Storage");
    }
}