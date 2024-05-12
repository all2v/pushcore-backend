using System.Data;

namespace Astrum.SharedLib.Application.Contracts.Infrastructure;

public interface IDbContext : Persistence.Repositories.IUnitOfWork
{
    void BeginTransaction();
    void Commit();
    void Rollback();
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync(IsolationLevel isolationLevel, CancellationToken cancellationToken = default);
    Task CommitAsync(CancellationToken cancellationToken = default);
}