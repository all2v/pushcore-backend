using Astrum.Market.Aggregates;
using Astrum.SharedLib.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Astrum.Market;

public class MarketDbContext : BaseDbContext
{
    public MarketDbContext(DbContextOptions<MarketDbContext> options) : base(options)
    {
    }

    public DbSet<MarketOrder> Orders { get; set; }
    public DbSet<MarketProduct> Products { get; set; }
    public DbSet<BasketProduct> BasketProducts { get; set; }
    public DbSet<OrderProduct> OrderProducts { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.HasDefaultSchema("Market");
    }
}