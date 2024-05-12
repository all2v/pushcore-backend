using Astrum.Infrastructure.Services.DbInitializer;
using Microsoft.EntityFrameworkCore;

namespace Astrum.Ordering;

public class DbContextContextInitializer : IDbContextInitializer
{
    private readonly OrderingDbContext _dbContext;

    public DbContextContextInitializer(OrderingDbContext dbContext)
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