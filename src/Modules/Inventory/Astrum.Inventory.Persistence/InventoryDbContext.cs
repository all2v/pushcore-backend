using Astrum.Inventory.Domain.Aggregates;
using Astrum.SharedLib.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Astrum.Inventory.Persistence
{
    public class InventoryDbContext : BaseDbContext
    {
        public DbSet<InventoryItem> InventoryItems { get; set; }
        public DbSet<Template> Templates { get; set; }
        public InventoryDbContext(DbContextOptions<InventoryDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.HasDefaultSchema("Inventories");
        }
    }
}
