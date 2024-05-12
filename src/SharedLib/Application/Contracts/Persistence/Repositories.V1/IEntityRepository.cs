using Astrum.Core.Domain.Interfaces;

namespace Astrum.Core.Application.Contracts.Persistence.Repositories.V1;

/// <summary>
///     Interface for generic repository for domain entities. See <see cref="IEntity{TId}" />.
///     Inherits from <see cref="IRepository{T,TId}" /> and <see cref="IWriteableRepository{T}" />
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="TId"></typeparam>
public interface IEntityRepository<T, TId> : IRepository<T, TId>
    where T : class, IEntity<TId>
{
}
