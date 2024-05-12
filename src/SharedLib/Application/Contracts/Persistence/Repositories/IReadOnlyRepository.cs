using System.Linq.Expressions;
using Ardalis.Specification;
using Sakura.AspNetCore;

namespace Astrum.SharedLib.Application.Contracts.Persistence.Repositories;

/// <summary>
///     Generic repository interface for read operations
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TId"></typeparam>
public interface IReadOnlyRepository<TEntity, in TId>
    where TEntity : class, Domain.Interfaces.IEntity<TId>
{
    [Obsolete]
    IQueryable<TEntity> Items { get; }

    /// <summary>
    ///     Retrieves all entities that satisfy the given condition from the data store as <see cref="IQueryable{T}" />
    /// </summary>
    /// <param name="specification">Condition</param>
    /// <returns>Query</returns>
    [Obsolete]
    IQueryable<TEntity> GetBy(ISpecification<TEntity> specification);

    /// <summary>
    ///     Retrieves all entities that satisfy the given condition from the data store as <see cref="IQueryable{T}" />
    /// </summary>
    /// <param name="criteria">Condition</param>
    /// <returns>Query</returns>
    [Obsolete]
    IQueryable<TEntity> GetBy(Expression<Func<TEntity, bool>> criteria);

    /// <summary>
    ///     Finds an entity from data store.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [Obsolete]
    TEntity GetById(TId id);

    /// <summary>
    ///     Asynchronously finds an entity from data store.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns></returns>
    [Obsolete]
    Task<TEntity> GetByIdAsync(TId id, CancellationToken cancellationToken = default);

    Task<List<TEntity>> ListAsync(CancellationToken cancellationToken = default);

    Task<List<TEntity>> ListAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default);

    [Obsolete]
    Task<List<TEntity>> ListAsync(Expression<Func<TEntity, bool>> criteria,
        CancellationToken cancellationToken = default);

    Task<IPagedList<TEntity>> PagedListAsync(int pageIndex, int pageSize, ISpecification<TEntity>? specification = null);

    /// <summary>
    ///     Checks if the data store contains any entity that satisfies the given predicate
    /// </summary>
    /// <param name="specification">Condition</param>
    /// <returns>True/false on whether an entity exists that satisfies the predicate</returns>
    [Obsolete]
    bool Any(ISpecification<TEntity> specification);

    /// <summary>
    ///     Checks if the data store contains any entity that satisfies the given predicate
    /// </summary>
    /// <param name="criteria">Condition</param>
    /// <returns>True/false on whether an entity exists that satisfies the predicate</returns>
    [Obsolete]
    bool Any(Expression<Func<TEntity, bool>> criteria);

    /// <summary>
    ///     Asynchronously checks if the data store contains any entity that satisfies the given predicate
    /// </summary>
    /// <param name="specification">Condition</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True/false on whether an entity exists that satisfies the predicate</returns>
    Task<bool> AnyAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Asynchronously checks if the data store contains any entity that satisfies the given predicate
    /// </summary>
    /// <param name="criteria">Condition</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True/false on whether an entity exists that satisfies the predicate</returns>
    [Obsolete]
    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> criteria, CancellationToken cancellationToken = default);

    Task<TEntity> FirstAsync(CancellationToken cancellationToken = default);

    Task<TEntity> FirstAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default);

    [Obsolete]
    Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> criteria, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Retrieves the first entity from data store that satisfies the given predicate
    /// </summary>
    /// <returns>Entity</returns>
    [Obsolete]
    TEntity? FirstOrDefault();

    /// <summary>
    ///     Retrieves the first entity from data store that satisfies the given predicate
    /// </summary>
    /// <param name="specification">Condition</param>
    /// <returns>Entity</returns>
    [Obsolete]
    TEntity? FirstOrDefault(ISpecification<TEntity> specification);

    /// <summary>
    ///     Retrieves the first entity from data store that satisfies the given predicate
    /// </summary>
    /// <param name="criteria">Condition</param>
    /// <returns>Entity</returns>
    [Obsolete]
    TEntity? FirstOrDefault(Expression<Func<TEntity, bool>> criteria);

    /// <summary>
    ///     Retrieves the first entity from data store that satisfies the given predicate
    /// </summary>
    /// <returns>Entity</returns>
    Task<TEntity?> FirstOrDefaultAsync(CancellationToken cancellationToken = default);

    /// <summary>
    ///     Asynchronously retrieves the first entity from the data store that satisfies the given predicate
    /// </summary>
    /// <param name="specification">Condition</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Entity</returns>
    Task<TEntity?> FirstOrDefaultAsync(ISpecification<TEntity> specification,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Asynchronously retrieves the first entity from the data store that satisfies the given predicate
    /// </summary>
    /// <param name="criteria">Condition</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Entity</returns>
    [Obsolete]
    Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> criteria,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Retrieves a single entity from data store that satisfies the given predicate
    /// </summary>
    /// <param name="specification">Condition</param>
    [Obsolete]
    TEntity Single(ISpecification<TEntity> specification);

    /// <summary>
    ///     Retrieves a single entity from data store that satisfies the given predicate
    /// </summary>
    /// <param name="criteria">Condition</param>
    [Obsolete]
    TEntity Single(Expression<Func<TEntity, bool>> criteria);

    /// <summary>
    ///     Asynchronously retrieves a single entity from data store that satisfies the given condition
    /// </summary>
    /// <param name="specification">Condition</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Entity</returns>
    Task<TEntity> SingleAsync(ISpecification<TEntity> specification,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Asynchronously retrieves a single entity from data store that satisfies the given condition
    /// </summary>
    /// <param name="criteria">Condition</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Entity</returns>
    [Obsolete]
    Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> criteria,
        CancellationToken cancellationToken = default);

    [Obsolete]
    int Count();

    Task<int> CountAsync(CancellationToken cancellationToken = default);
    Task<int> CountAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default);

    [Obsolete]
    Task<int> CountAsync(Expression<Func<TEntity, bool>> criteria, CancellationToken cancellationToken = default);

    long LongCount();
    Task<long> LongCountAsync(CancellationToken cancellationToken = default);
    Task<long> LongCountAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default);

    [Obsolete]
    Task<long> LongCountAsync(Expression<Func<TEntity, bool>> criteria, CancellationToken cancellationToken = default);
}