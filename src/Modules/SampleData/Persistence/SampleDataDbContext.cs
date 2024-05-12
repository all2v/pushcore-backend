using Astrum.SharedLib.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Astrum.SampleData.Persistence;

public class SampleDataDbContext : BaseDbContext
{
    public SampleDataDbContext(DbContextOptions<SampleDataDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}
