namespace Astrum.Core.Application.Contracts.Persistence.Repositories.V1;

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
    T Add(T entity);

    /// <summary>
    ///     Asynchronously adds the entity to the data store
    /// </summary>
    /// <param name="entity">The entity to add</param>
    ValueTask<T> AddAsync(T entity, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Adds a range of entities to the data store
    /// </summary>
    /// <param name="entities">The collection of entities to add</param>
    void AddRange(IEnumerable<T> entities);

    /// <summary>
    ///     Asychronously adds a range of entities to the data store
    /// </summary>
    /// <param name="entities">The collection of entities to add</param>
    Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Deletes an entity from the data store
    /// </summary>
    /// <param name="entity">The entity to delete</param>
    void Delete(T entity);

    /// <summary>
    ///     Deletes a range of entities from the data store
    /// </summary>
    /// <param name="entities">The collection of entities to delete</param>
    void DeleteRange(IEnumerable<T> entities);

    /// <summary>
    ///     Updates an entity in the data store
    /// </summary>
    /// <param name="entity">The entity to update</param>
    T Update(T entity);

    /// <summary>
    ///     Updates a range of entities in the data store
    /// </summary>
    /// <param name="entities">The collection of entities to update</param>
    void UpdateRange(IEnumerable<T> entities);
}