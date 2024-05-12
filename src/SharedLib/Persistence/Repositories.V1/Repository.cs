using Astrum.Core.Application.Contracts.Persistence.Repositories.V1;
using IUnitOfWork = Astrum.Core.Application.Contracts.Persistence.Repositories.IUnitOfWork;

namespace Astrum.Core.Persistence.Repositories.V1;

/// <summary>
///     Inherits <see cref="ReadOnlyRepository{TEntity,TId}" /> for convenience.
/// </summary>
public abstract class Repository<T, TId> : ReadOnlyRepository<T, TId>, IRepository<T, TId>
{
    private readonly IUnitOfWork _unitOfWork;

    protected Repository(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        ReadOnly = false;
    }

    #region IRepository<T,TId> Members

    public IUnitOfWork UnitOfWork
    {
        get
        {
            if (ReadOnly) throw new NotImplementedException("For current repository enabled readonly mode");

            return _unitOfWork;
        }
    }

    public abstract T Add(T entity);

    public abstract ValueTask<T> AddAsync(T entity, CancellationToken cancellationToken);

    public abstract void AddRange(IEnumerable<T> entities);

    public abstract Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken);

    public abstract void Delete(T entity);

    public abstract void DeleteRange(IEnumerable<T> entities);

    public abstract T Update(T entity);

    public abstract void UpdateRange(IEnumerable<T> entities);

    #endregion
}