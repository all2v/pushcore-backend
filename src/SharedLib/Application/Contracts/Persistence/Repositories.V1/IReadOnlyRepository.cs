using Astrum.Core.Domain.Specifications;

namespace Astrum.Core.Application.Contracts.Persistence.Repositories.V1;

/// <summary>
///     Generic repository interface for read operations
/// </summary>
/// <typeparam name="TItem"></typeparam>
/// <typeparam name="TId"></typeparam>
public interface IReadOnlyRepository<TItem, in TId>
{
    bool ReadOnly { get; }
    IQueryable<TItem> Items { get; }

    /// <summary>
    ///     Retrieves all entities that satisfy the given condition from the data store as <see cref="IQueryable{T}" />
    /// </summary>
    /// <param name="specification">Condition</param>
    /// <returns>Query</returns>
    IQueryable<TItem> GetBy(ISpecification<TItem> specification);

    /// <summary>
    ///     Finds an entity from data store.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    TItem Find(TId id);

    /// <summary>
    ///     Asynchronously finds an entity from data store.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<TItem> FindAsync(TId id, CancellationToken cancellationToken = default);


    Task<List<TItem>> ListAsync(CancellationToken cancellationToken = default);
    Task<List<TItem>> ListAsync(ISpecification<TItem> specification, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Checks if the data store contains any entity that satisfies the given predicate
    /// </summary>
    /// <param name="specification">Condition</param>
    /// <returns>True/false on whether an entity exists that satisfies the predicate</returns>
    bool Exists(ISpecification<TItem> specification);

    /// <summary>
    ///     Asynchronously checks if the data store contains any entity that satisfies the given predicate
    /// </summary>
    /// <param name="specification">Condition</param>
    /// <param name="cancellationToken"></param>
    /// <returns>True/false on whether an entity exists that satisfies the predicate</returns>
    Task<bool> ExistsAsync(ISpecification<TItem> specification, CancellationToken cancellationToken = default);

    Task<TItem> FirstAsync(CancellationToken cancellationToken);
    Task<TItem> FirstAsync(ISpecification<TItem> specification, CancellationToken cancellationToken = new());

    /// <summary>
    ///     Retrieves the first entity from data store that satisfies the given predicate
    /// </summary>
    /// <param name="specification">Condition</param>
    /// <returns>Entity</returns>
    TItem? FirstOrDefault(ISpecification<TItem> specification);

    /// <summary>
    ///     Asynchronously retrieves the first entity from the data store that satisfies the given predicate
    /// </summary>
    /// <param name="specification">Condition</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Entity</returns>
    Task<TItem?> FirstOrDefaultAsync(ISpecification<TItem> specification, bool isTracked = false,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Retrieves a single entity from data store that satisfies the given predicate
    /// </summary>
    /// <param name="specification">Condition</param>
    TItem GetSingle(ISpecification<TItem> specification);

    /// <summary>
    ///     Asynchronously retrieves a single entity from data store that satisfies the given condition
    /// </summary>
    /// <param name="specification">Condition</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Entity</returns>
    Task<TItem> GetSingleAsync(ISpecification<TItem> specification, CancellationToken cancellationToken = default);

    public Task<int> CountAsync(CancellationToken cancellationToken = default);

    public Task<int> CountAsync(ISpecification<TItem> specification, CancellationToken cancellationToken = default);

    Task<long> LongCountAsync(CancellationToken cancellationToken = default);
    Task<long> LongCountAsync(ISpecification<TItem> specification, CancellationToken cancellationToken = default);


    Task<TResult[]> Query<TResult>(Func<IQueryable<TItem>, IQueryable<TResult>> query,
        CancellationToken cancellationToken = default);

    IQueryable<TItem> Specify(ISpecification<TItem> specification,
        CancellationToken cancellationToken = default);
}