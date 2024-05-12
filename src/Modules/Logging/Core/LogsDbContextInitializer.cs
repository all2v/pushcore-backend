using Astrum.Infrastructure.Services.DbInitializer;
using Microsoft.EntityFrameworkCore;

namespace Astrum.Logging
{
    public class LogsDbContextInitializer : IDbContextInitializer
    {
        private readonly LogsDbContext _appealDbContext;

        public LogsDbContextInitializer(LogsDbContext appealDbContext)
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
}
