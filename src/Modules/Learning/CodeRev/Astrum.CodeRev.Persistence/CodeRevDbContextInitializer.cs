using Astrum.Infrastructure.Services.DbInitializer;
using Microsoft.EntityFrameworkCore;

namespace Astrum.CodeRev.Persistence;

public class CodeRevDbContextInitializer : IDbContextInitializer
{
    private readonly CodeRevDbContext _dbContext;

    public CodeRevDbContextInitializer(CodeRevDbContext dbContext)
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