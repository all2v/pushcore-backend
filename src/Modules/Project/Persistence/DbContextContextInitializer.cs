using Astrum.Infrastructure.Services.DbInitializer;
using Microsoft.EntityFrameworkCore;

namespace Astrum.Projects;

public class DbContextInitializer : IDbContextInitializer
{
    private readonly ProjectDbContext _dbContext;

    public DbContextInitializer(ProjectDbContext dbContext)
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