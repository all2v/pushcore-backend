using Astrum.Infrastructure.Services.DbInitializer;
using Microsoft.EntityFrameworkCore;

namespace Astrum.Calendar;

public class DbContextContextInitializer : IDbContextInitializer
{
    private readonly CalendarDbContext _dbContext;

    public DbContextContextInitializer(CalendarDbContext dbContext)
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