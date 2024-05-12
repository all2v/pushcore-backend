using Astrum.Core.Domain.Interfaces;

namespace Astrum.Core.Application.Contracts.Persistence.Repositories.V1;

/// <summary>
///     Generic marker interface for repository for aggregate root entities
///     Inherits from <see cref="IEntityRepository{TAggregateRoot, TId}" />  />
/// </summary>
/// <typeparam name="TAggregateRoot"></typeparam>
/// <typeparam name="TId"></typeparam>
public interface IAggregateRepository<TAggregateRoot, TId> : IEntityRepository<TAggregateRoot, TId>
    where TAggregateRoot : class, IAggregateRoot<TId>
{
}