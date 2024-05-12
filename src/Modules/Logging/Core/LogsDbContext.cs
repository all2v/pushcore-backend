using Astrum.Logging.Entities;
using Astrum.SharedLib.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Astrum.Logging
{
    public class LogsDbContext : BaseDbContext
    {
        public LogsDbContext(DbContextOptions<LogsDbContext> options) : base(options)
        {
        }

        public DbSet<LogHttp> HttpDataLogs { get; set; }
        public DbSet<LogAdmin> AdminDataLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.HasDefaultSchema("Logs");
        }
    }
}
