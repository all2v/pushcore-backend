namespace Astrum.Core.Application.Contracts.Persistence.Repositories.V1;

/// <summary>
///     Interface for generic repository.
///     Inherits from <see cref="Repositories.IReadOnlyRepository{TEntity,TId}" /> and <see cref="IWriteableRepository{T}" />
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="TId"></typeparam>
public interface IRepository<T, in TId> : IWriteableRepository<T>, IReadOnlyRepository<T, TId>
{
}