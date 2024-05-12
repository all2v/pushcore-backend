using Astrum.Infrastructure.Services.DbInitializer;
using Microsoft.EntityFrameworkCore;

namespace Astrum.News;

public class DbContextContextInitializer : IDbContextInitializer
{
    private readonly NewsDbContext _dbContext;

    public DbContextContextInitializer(NewsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    #region IDbContextInitializer Members

    public async Task Migrate(CancellationToken cancellationToken = default)
    {
        await _dbContext.Database.MigrateAsync(cancellationToken);
    }

    public Task Seed(CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    #endregion
}