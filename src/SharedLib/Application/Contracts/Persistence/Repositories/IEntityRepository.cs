using Astrum.SharedLib.Domain.Interfaces;

namespace Astrum.SharedLib.Application.Contracts.Persistence.Repositories;

/// <summary>
///     Interface for generic repository for domain entities. See <see cref="IEntity{TId}" />.
///     Inherits from <see cref="IWriteableRepository{T}" /> and <see cref="Application" />
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TId"></typeparam>
public interface IEntityRepository<TEntity, in TId> : IWriteableRepository<TEntity>, IReadOnlyRepository<TEntity, TId>
    where TEntity : class, IEntity<TId>
{
}
