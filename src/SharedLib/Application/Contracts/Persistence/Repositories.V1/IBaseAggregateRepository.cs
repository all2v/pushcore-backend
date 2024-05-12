using Astrum.Core.Domain.Interfaces;

namespace Astrum.Core.Application.Contracts.Persistence.Repositories.V1;

/// <summary>
///     Generic marker interface for aggregate root entities repository with separated Data entity type
/// </summary>
/// Inherits from
/// <see cref="IAggregateRepository{TAggregateRoot, TId}" />
/// />
/// <typeparam name="TAggregateRoot"></typeparam>
/// <typeparam name="TDataEntity"></typeparam>
/// <typeparam name="TId"></typeparam>
public interface IBaseAggregateRepository<TAggregateRoot, TDataEntity, TId> : IAggregateRepository<TAggregateRoot, TId>
    where TAggregateRoot : class, IAggregateRoot<TId>
    where TDataEntity : class, IDataEntity<TId>
{
    public TDataEntity ToDataEntity(TAggregateRoot aggregateRoot);
    public TAggregateRoot ToAggregateRoot(TDataEntity entity);
}