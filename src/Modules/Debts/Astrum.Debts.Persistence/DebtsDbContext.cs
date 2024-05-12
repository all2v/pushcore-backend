using Astrum.Debts.Domain.Aggregates;
using Astrum.SharedLib.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Astrum.Debts.Persistence
{
    public class DebtsDbContext : BaseDbContext
    {
        public DbSet<Debt> Debts { get; set; }
        public DebtsDbContext(DbContextOptions<DebtsDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.HasDefaultSchema("Debts");
        }
    }
}
