using Astrum.Ordering.Aggregates;
using Astrum.SharedLib.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Astrum.Ordering;

public class OrderingDbContext : BaseDbContext
{
    public OrderingDbContext(DbContextOptions<OrderingDbContext> options) : base(options)
    {
    }

    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.HasDefaultSchema("Ordering");
    }
}