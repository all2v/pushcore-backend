using Astrum.SampleData.Aggregates;
using Astrum.SharedLib.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Astrum.SampleData.Persistence;

public class SampleContentDbContext : BaseDbContext
{
    public SampleContentDbContext(DbContextOptions<SampleContentDbContext> options) : base(options)
    { 
    }
    public DbSet<SampleContentFile> SampleContentFile { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.HasDefaultSchema("SampleContent");
    }
}
