using Astrum.Infrastructure.Services.DbInitializer;
using Microsoft.EntityFrameworkCore;

namespace Astrum.TrackerProject.Persistance;

public class DbContextContextInitializer : IDbContextInitializer
{
    private readonly TrackerProjectDbContext _dbContext;

    public DbContextContextInitializer(TrackerProjectDbContext dbContext)
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