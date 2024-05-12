namespace Astrum.Infrastructure.Services.DbInitializer;

public interface IDbContextInitializer
{
    Task Migrate(CancellationToken cancellationToken = default);
    Task Seed(CancellationToken cancellationToken = default);
}