using Astrum.Account.Domain.Aggregates;
using Astrum.Identity.Models;
using Astrum.Infrastructure.Services.DbInitializer;
using Microsoft.EntityFrameworkCore;

namespace Astrum.Account;

public class DbContextContextInitializer : IDbContextInitializer
{
    private readonly AccountDbContext _dbContext;

    public DbContextContextInitializer(AccountDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    #region IDbContextInitializer Members

    public async Task Migrate(CancellationToken cancellationToken = default)
    {
        await _dbContext.Database.MigrateAsync(cancellationToken);
    }

    public async Task Seed(CancellationToken cancellationToken = default)
    {
        await SeedPositionsAsync();
    }

    private async Task SeedPositionsAsync()
    {
        var positions = _dbContext.Positions;
        if (!positions.Any())
        {
            positions.Add(new Position
            {
                Name = "Дизайнер"
            });
            positions.Add(new Position
            {
                Name = "Менеджер"
            });
            positions.Add(new Position
            {
                Name = "Аналитик"
            });
            positions.Add(new Position
            {
                Name = "Backend"
            });
            positions.Add(new Position
            {
                Name = "Frontend"
            });
            positions.Add(new Position
            {
                Name = "Fullstack"
            });
            await _dbContext.SaveChangesAsync();
        }
    }

    #endregion
}