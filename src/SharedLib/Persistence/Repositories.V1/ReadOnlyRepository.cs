using Astrum.Core.Application.Contracts.Persistence.Repositories.V1;
using Astrum.Core.Domain.Specifications;

namespace Astrum.Core.Persistence.Repositories.V1;

/// <summary>
///     Inherits <see cref="IReadOnlyRepository{TItem,TId}" /> for convenience.
/// </summary>
public abstract class ReadOnlyRepository<TEntity, TId> : IReadOnlyRepository<TEntity, TId>
{
    #region IReadOnlyRepository<TEntity,TId> Members

    public bool ReadOnly { get; protected init; }
    public abstract IQueryable<TEntity> Items { get; }
    public abstract IQueryable<TEntity> GetBy(ISpecification<TEntity> specification);
    public abstract TEntity Find(TId id);
    public abstract Task<TEntity> FindAsync(TId id, CancellationToken cancellationToken = default);
    public abstract Task<List<TEntity>> ListAsync(CancellationToken cancellationToken = default);

    public abstract Task<List<TEntity>> ListAsync(ISpecification<TEntity> specification,
        CancellationToken cancellationToken = default);

    public abstract bool Exists(ISpecification<TEntity> specification);

    public abstract Task<bool> ExistsAsync(ISpecification<TEntity> specification,
        CancellationToken cancellationToken = default);

    public abstract Task<TEntity> FirstAsync(CancellationToken cancellationToken);

    public abstract Task<TEntity> FirstAsync(ISpecification<TEntity> specification,
        CancellationToken cancellationToken = new());

    public abstract TEntity? FirstOrDefault(ISpecification<TEntity> specification);

    public abstract Task<TEntity?> FirstOrDefaultAsync(ISpecification<TEntity> specification, bool isTracked = false,
        CancellationToken cancellationToken = default);

    public abstract TEntity GetSingle(ISpecification<TEntity> specification);

    public abstract Task<TEntity> GetSingleAsync(ISpecification<TEntity> specification,
        CancellationToken cancellationToken = default);

    public abstract Task<int> CountAsync(CancellationToken cancellationToken = default);

    public abstract Task<int> CountAsync(ISpecification<TEntity> specification,
        CancellationToken cancellationToken = default);

    public abstract Task<long> LongCountAsync(CancellationToken cancellationToken = default);

    public abstract Task<long> LongCountAsync(ISpecification<TEntity> specification,
        CancellationToken cancellationToken = default);

    public abstract Task<TResult[]> Query<TResult>(Func<IQueryable<TEntity>, IQueryable<TResult>> query,
        CancellationToken cancellationToken = default);
    public abstract IQueryable<TEntity> Specify(ISpecification<TEntity> specification,
        CancellationToken cancellationToken = default);

    #endregion
}