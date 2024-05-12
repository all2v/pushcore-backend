using Astrum.Infrastructure.Services.DbInitializer;
using Microsoft.EntityFrameworkCore;

namespace Astrum.Recruting.Persistence;

public class RecrutingDbContextInitializer : IDbContextInitializer
{
    private readonly RecrutingDbContext _context;

    public RecrutingDbContextInitializer(RecrutingDbContext context)
    {
        _context = context;
    }

    public async Task Migrate(CancellationToken cancellationToken = default)
    {
        await _context.Database.MigrateAsync(cancellationToken);
    }

    public Task Seed(CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}