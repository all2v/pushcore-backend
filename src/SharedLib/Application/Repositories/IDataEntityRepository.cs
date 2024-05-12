using Astrum.Core.Application.Contracts.Infrastructure;
using Astrum.Core.Application.Contracts.Persistence.Repositories;
using Astrum.Core.Domain.Interfaces;

namespace Astrum.Core.Application.Repositories;

/// <inheritdoc cref="Contracts.Persistence.Repositories.V1.IEntityRepository{T,TId}" />
/// <summary>
///     Repository for data entities. See <see cref="IDataEntity{TId}" />. Implements
///     <see cref="Contracts.Persistence.Repositories.V1.IEntityRepository{T,TId}" />
///     Used only for application-specific business entities, in case you don't need them in your business domain model
///     (e.x. Application Configurations)
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TId"></typeparam>
public interface IDataEntityRepository<TEntity, TId> : Contracts.Persistence.Repositories.V1.IEntityRepository<TEntity, TId>
    where TEntity : class, IDataEntity<TId>
{
}