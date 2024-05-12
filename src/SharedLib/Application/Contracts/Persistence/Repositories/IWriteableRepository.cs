

namespace Astrum.SharedLib.Application.Contracts.Persistence.Repositories;

/// <summary>
///     Generic repository interface for write operations.
///     Includes a <see cref="IUnitOfWork" />
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IWriteableRepository<T> : IRepository
{
    IUnitOfWork UnitOfWork { get; }

    /// <summary>
    ///     Adds the entity to the data store
    /// </summary>
    /// <param name="entity">The entity to add</param>
    /// <returns>The changed entity</returns>
    [Obsolete]
    T Add(T entity);

    /// <summary>
    ///     Asynchronously adds the entity to the data store
    /// </summary>
    /// <param name="entity">The entity to add</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Adds a range of entities to the data store
    /// </summary>
    /// <param name="entities">The collection of entities to add</param>
    [Obsolete]
    void AddRange(IEnumerable<T> entities);

    /// <summary>
    ///     Asynchronously adds a range of entities to the data store
    /// </summary>
    /// <param name="entities">The collection of entities to add</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Deletes an entity from the data store
    /// </summary>
    /// <param name="entity">The entity to delete</param>
    /// <param name="cancellationToken"></param>
    Task DeleteAsync(T entity, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Deletes a range of entities from the data store
    /// </summary>
    /// <param name="entities">The collection of entities to delete</param>
    /// <param name="cancellationToken"></param>
    Task DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Updates an entity in the data store
    /// </summary>
    /// <param name="entity">The entity to update</param>
    /// <param name="cancellationToken"></param>
    Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Updates a range of entities in the data store
    /// </summary>
    /// <param name="entities">The collection of entities to update</param>
    /// <param name="cancellationToken"></param>
    Task UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
}