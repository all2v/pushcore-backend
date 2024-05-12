using Astrum.Infrastructure.Services.DbInitializer;
using Microsoft.EntityFrameworkCore;

namespace Astrum.Appeal;

public class AppealDbContextContextInitializer : IDbContextInitializer
{
    private readonly AppealDbContext _appealDbContext;

    public AppealDbContextContextInitializer(AppealDbContext appealDbContext)
    {
        _appealDbContext = appealDbContext;
    }

    #region IDbContextInitializer Members

    public async Task Migrate(CancellationToken cancellationToken = default)
    {
        await _appealDbContext.Database.MigrateAsync(cancellationToken);
    }

    public Task Seed(CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    #endregion
}