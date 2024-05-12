using System.Runtime.CompilerServices;
using Astrum.Application.Entities;
using Astrum.SharedLib.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

[assembly: InternalsVisibleTo("Astrum.Tests")]

namespace Astrum.Application;

public class EventStoreDbContext : BaseDbContext
{
    public EventStoreDbContext(DbContextOptions<EventStoreDbContext> options) : base(options)
    {
    }

    public DbSet<Event> Events { get; set; }
    public DbSet<AggregateSnapshot> Snapshots { get; set; }
    public DbSet<BranchPoint> BranchPoints { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.HasDefaultSchema("EventStore");
    }
}