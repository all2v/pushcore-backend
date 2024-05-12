// using Ardalis.Specification;
// using Astrum.SharedLib.Application.Contracts.Persistence.Repositories;
//
// namespace Astrum.SharedLib.Application.Repositories;
//
// /// <summary>
// ///     Inherits <see cref="ReadOnlyRepository{TEntity,TId}" /> for convenience.
// /// </summary>
// public abstract class Repository<TEntity, TId> : ReadOnlyRepository<TEntity, TId>, Contracts.Persistence.Repositories.IEntityRepository<TEntity, TId>
//     where TEntity : class, Domain.Interfaces.IEntity<TId>
// {
//     protected Repository(IUnitOfWork unitOfWork, ISpecificationEvaluator specificationEvaluator)
//         : base(specificationEvaluator)
//     {
//         UnitOfWork = unitOfWork;
//     }
//
//     #region IEntityRepository<TEntity,TId> Members
//
//     public IUnitOfWork UnitOfWork { get; }
//
//     public abstract TEntity Add(TEntity entity);
//
//     public abstract Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken);
//
//     public abstract void AddRange(IEnumerable<TEntity> entities);
//
//     public abstract Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken);
//
//     public abstract Task DeleteAsync(TEntity entity);
//
//     public abstract Task DeleteRangeAsync(IEnumerable<TEntity> entities);
//
//     public abstract Task<TEntity> UpdateAsync(TEntity entity);
//     public abstract Task UpdateRangeAsync(IEnumerable<TEntity> entities);
//
//     #endregion
// }