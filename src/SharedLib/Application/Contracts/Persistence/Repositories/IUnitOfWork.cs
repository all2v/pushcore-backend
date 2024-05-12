namespace Astrum.SharedLib.Application.Contracts.Persistence.Repositories;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default, bool ensureAudit = true);

    Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default, bool ensureAudit = true);

    int SaveChanges();

    int SaveChanges(bool acceptAllChangesOnSuccess);
}