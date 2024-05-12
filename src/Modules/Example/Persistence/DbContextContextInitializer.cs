using Astrum.Infrastructure.Services.DbInitializer;
using Microsoft.EntityFrameworkCore;

namespace Astrum.Example;

public class DbContextContextInitializer : IDbContextInitializer
{
    private readonly ExampleDbContext _dbContext;

    public DbContextContextInitializer(ExampleDbContext dbContext)
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