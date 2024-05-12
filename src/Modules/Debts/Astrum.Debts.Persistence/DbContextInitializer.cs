using Astrum.Infrastructure.Services.DbInitializer;
using Microsoft.EntityFrameworkCore;

namespace Astrum.Debts.Persistence
{
    public class DbContextInitializer : IDbContextInitializer
    {
        private readonly DebtsDbContext _dbContext;
        public DbContextInitializer(DebtsDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task Migrate(CancellationToken cancellationToken = default)
        {
            await _dbContext.Database.MigrateAsync(cancellationToken);
        }

        public Task Seed(CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }
}
