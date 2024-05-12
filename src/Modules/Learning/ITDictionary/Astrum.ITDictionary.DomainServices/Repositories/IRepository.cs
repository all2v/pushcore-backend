using Ardalis.Specification;
using Astrum.SharedLib.Application.Contracts.Persistence.Repositories;

namespace Astrum.ITDictionary.Repositories;

public interface IRepository<TEntity> : IWriteableRepository<TEntity>
    where TEntity : class
{
    Task<List<TEntity>> ListAsync(CancellationToken cancellationToken = default);

    Task<List<TEntity>> ListAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default);

    Task<bool> AnyAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default);

    Task<TEntity> FirstAsync(CancellationToken cancellationToken = default);

    Task<TEntity> FirstAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default);

    Task<TEntity?> FirstOrDefaultAsync(CancellationToken cancellationToken = default);

    Task<TEntity?> FirstOrDefaultAsync(ISpecification<TEntity> specification,
        CancellationToken cancellationToken = default);

    Task<TEntity> SingleAsync(ISpecification<TEntity> specification,
        CancellationToken cancellationToken = default);

    Task<int> CountAsync(CancellationToken cancellationToken = default);

    Task<int> CountAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default);

    long LongCount();

    Task<long> LongCountAsync(CancellationToken cancellationToken = default);

    Task<long> LongCountAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default);
}