// using System.Linq.Expressions;
// using Ardalis.GuardClauses;
// using Ark.SharedLib.Application.Contracts.Persistence.Repositories;
// using Ark.SharedLib.Application.Repositories;
// using Ark.SharedLib.Domain.Interfaces;
// using Ark.SharedLib.Domain.Specifications;
// using AutoMapper;
// using AutoMapper.Extensions.ExpressionMapping;
//
// namespace Ark.Persistence.Repositories;
//
// /// <inheritdoc cref="IAggregateRepository{TAggregateRoot,TId}" />
// /// <summary>
// ///     Repository implementation of <see cref="IAggregateRepository{TAggregateRoot,TId}" />.
// ///     Operates on <see cref="IAggregateRoot{TId}" /> by having the underlying data entity of
// ///     <see cref="IDataEntity{TId}" />
// /// </summary>
// public abstract class AggregateRepository<TAggregateRoot, TDataEntity, TId>
//     : IAggregateRepository<TAggregateRoot, TId>
//     where TAggregateRoot : class, IAggregateRoot<TId>
//     where TDataEntity : class, IDataEntity<TId>
// {
//     /// <summary>
//     /// </summary>
//     protected readonly IDataEntityRepository<TDataEntity, TId> DataOrderEntityRepository;
//
//     /// <summary>
//     /// </summary>
//     protected readonly IMapper Mapper;
//
//     /// <summary>
//     ///     Constructor
//     /// </summary>
//     /// <param name="mapper"></param>
//     /// <param name="dataOrderEntityRepository"></param>
//     protected AggregateRepository(IMapper mapper, IDataEntityRepository<TDataEntity, TId> dataOrderEntityRepository)
//     {
//         Mapper = mapper;
//         DataOrderEntityRepository = dataOrderEntityRepository;
//     }
//
//     #region IAggregateRepository<TAggregateRoot,TId> Members
//
//     public IUnitOfWork UnitOfWork => DataOrderEntityRepository.UnitOfWork;
//
//     // public virtual TAggregateRoot Add(TAggregateRoot entity)
//     // {
//     //     Guard.Against.Null(entity, nameof(entity));
//     //
//     //     var dataEntity = Mapper.Map<TDataEntity>(entity);
//     //     dataEntity = DataOrderEntityRepository.Add(dataEntity);
//     //     return Mapper.Map<TAggregateRoot>(dataEntity);
//     // }
//
//     public virtual async Task<TAggregateRoot> AddAsync(TAggregateRoot entity,
//         CancellationToken cancellationToken = default)
//     {
//         Guard.Against.Null(entity, nameof(entity));
//
//         var dataEntity = Mapper.Map<TDataEntity>(entity);
//         dataEntity = await DataOrderEntityRepository.AddAsync(dataEntity, cancellationToken);
//         return Mapper.Map(dataEntity, entity);
//     }
//
//     // public virtual void AddRange(IEnumerable<TAggregateRoot> entities)
//     // {
//     //     var dataEntities = Mapper.Map<IEnumerable<TDataEntity>>(entities);
//     //     DataOrderEntityRepository.AddRange(dataEntities);
//     // }
//
//     public async Task AddRangeAsync(IEnumerable<TAggregateRoot> entities, CancellationToken cancellationToken = default)
//     {
//         var dataEntities = Mapper.Map<IEnumerable<TDataEntity>>(entities);
//         await DataOrderEntityRepository.AddRangeAsync(dataEntities, cancellationToken);
//     }
//
//     public async Task<List<TAggregateRoot>> ListAsync(ISpecification<TAggregateRoot> specification,
//         CancellationToken cancellationToken = default)
//     {
//         var dataEntitySpecification = Mapper.Map<ISpecification<TDataEntity>>(specification);
//         var entityList =
//             await DataOrderEntityRepository.ListAsync(dataEntitySpecification, cancellationToken);
//         return Mapper.Map<List<TAggregateRoot>>(entityList);
//     }
//
//     public async Task<List<TAggregateRoot>> ListAsync(Expression<Func<TAggregateRoot, bool>> criteria,
//         CancellationToken cancellationToken = default)
//     {
//         var dataEntityCriteria = Mapper.MapExpression<Expression<Func<TDataEntity, bool>>>(criteria);
//         var entityList = await DataOrderEntityRepository.ListAsync(dataEntityCriteria, cancellationToken);
//         return Mapper.Map<List<TAggregateRoot>>(entityList);
//     }
//
//     public async Task<List<TAggregateRoot>> ListAsync(
//         CancellationToken cancellationToken = default)
//     {
//         var entityList = await DataOrderEntityRepository.ListAsync(cancellationToken);
//         return Mapper.Map<List<TAggregateRoot>>(entityList);
//     }
//
//     public bool Any(ISpecification<TAggregateRoot> specification)
//     {
//         var dataEntitySpecification = Mapper.Map<ISpecification<TDataEntity>>(specification);
//         return DataOrderEntityRepository.Any(dataEntitySpecification);
//     }
//
//     public bool Any(Expression<Func<TAggregateRoot, bool>> criteria)
//     {
//         var dataEntityCriteria = Mapper.MapExpression<Expression<Func<TDataEntity, bool>>>(criteria);
//         return DataOrderEntityRepository.Any(dataEntityCriteria);
//     }
//
//     public async Task<bool> AnyAsync(ISpecification<TAggregateRoot> specification,
//         CancellationToken cancellationToken = default)
//     {
//         var dataEntitySpecification = Mapper.Map<ISpecification<TDataEntity>>(specification);
//         return await DataOrderEntityRepository.AnyAsync(dataEntitySpecification, cancellationToken);
//     }
//
//     public Task<bool> AnyAsync(Expression<Func<TAggregateRoot, bool>> criteria,
//         CancellationToken cancellationToken = default)
//     {
//         var dataEntityCriteria = Mapper.MapExpression<Expression<Func<TDataEntity, bool>>>(criteria);
//         return DataOrderEntityRepository.AnyAsync(dataEntityCriteria, cancellationToken);
//     }
//
//     public async Task<TAggregateRoot> FirstAsync(CancellationToken cancellationToken = default)
//     {
//         var entity = await DataOrderEntityRepository.FirstAsync(cancellationToken);
//         return Mapper.Map<TAggregateRoot>(entity);
//     }
//
//     public async Task<TAggregateRoot> FirstAsync(ISpecification<TAggregateRoot> specification,
//         CancellationToken cancellationToken = default)
//     {
//         var dataEntitySpecification = Mapper.Map<ISpecification<TDataEntity>>(specification);
//         var entity = await DataOrderEntityRepository.FirstAsync(dataEntitySpecification, cancellationToken);
//         return Mapper.Map<TAggregateRoot>(entity);
//     }
//
//     public async Task<TAggregateRoot> FirstAsync(Expression<Func<TAggregateRoot, bool>> criteria,
//         CancellationToken cancellationToken = default)
//     {
//         var dataEntityCriteria = Mapper.MapExpression<Expression<Func<TDataEntity, bool>>>(criteria);
//         var entity = await DataOrderEntityRepository.FirstAsync(dataEntityCriteria, cancellationToken);
//         return Mapper.Map<TAggregateRoot>(entity);
//     }
//
//     public TAggregateRoot? FirstOrDefault()
//     {
//         var entity = DataOrderEntityRepository.FirstOrDefault();
//         return Mapper.Map<TAggregateRoot>(entity);
//     }
//
//     public TAggregateRoot? FirstOrDefault(ISpecification<TAggregateRoot> specification)
//     {
//         var dataEntitySpecification = Mapper.Map<ISpecification<TDataEntity>>(specification);
//         var entity = DataOrderEntityRepository.FirstOrDefault(dataEntitySpecification);
//         return Mapper.Map<TAggregateRoot>(entity);
//     }
//
//     public TAggregateRoot? FirstOrDefault(Expression<Func<TAggregateRoot, bool>> criteria)
//     {
//         var dataEntityCriteria = Mapper.MapExpression<Expression<Func<TDataEntity, bool>>>(criteria);
//         var entity = DataOrderEntityRepository.FirstOrDefault(dataEntityCriteria);
//         return Mapper.Map<TAggregateRoot>(entity);
//     }
//
//     public async Task<TAggregateRoot?> FirstOrDefaultAsync(ISpecification<TAggregateRoot> specification,
//         CancellationToken cancellationToken = default)
//     {
//         var dataEntitySpecification = Mapper.Map<ISpecification<TDataEntity>>(specification);
//         var entity =
//             await DataOrderEntityRepository.FirstOrDefaultAsync(dataEntitySpecification, cancellationToken);
//         return Mapper.Map<TAggregateRoot>(entity);
//     }
//
//     public async Task<TAggregateRoot?> FirstOrDefaultAsync(Expression<Func<TAggregateRoot, bool>> criteria,
//         CancellationToken cancellationToken = default)
//     {
//         var dataEntityCriteria = Mapper.MapExpression<Expression<Func<TDataEntity, bool>>>(criteria);
//         var entity =
//             await DataOrderEntityRepository.FirstOrDefaultAsync(dataEntityCriteria, cancellationToken);
//         return Mapper.Map<TAggregateRoot>(entity);
//     }
//
//     public TAggregateRoot Single(ISpecification<TAggregateRoot> specification)
//     {
//         var dataEntitySpecification = Mapper.Map<ISpecification<TDataEntity>>(specification);
//         var entity = DataOrderEntityRepository.Single(dataEntitySpecification);
//         return Mapper.Map<TAggregateRoot>(entity);
//     }
//
//     public TAggregateRoot Single(Expression<Func<TAggregateRoot, bool>> criteria)
//     {
//         var dataEntityCriteria = Mapper.MapExpression<Expression<Func<TDataEntity, bool>>>(criteria);
//         var entity = DataOrderEntityRepository.Single(dataEntityCriteria);
//         return Mapper.Map<TAggregateRoot>(entity);
//     }
//
//     public async Task<TAggregateRoot> SingleAsync(ISpecification<TAggregateRoot> specification,
//         CancellationToken cancellationToken = default)
//     {
//         var dataEntitySpecification = Mapper.Map<ISpecification<TDataEntity>>(specification);
//         var entity =
//             await DataOrderEntityRepository.SingleAsync(dataEntitySpecification, cancellationToken);
//         return Mapper.Map<TAggregateRoot>(entity);
//     }
//
//     public async Task<TAggregateRoot> SingleAsync(Expression<Func<TAggregateRoot, bool>> criteria,
//         CancellationToken cancellationToken = default)
//     {
//         var dataEntityCriteria = Mapper.MapExpression<Expression<Func<TDataEntity, bool>>>(criteria);
//         var entity =
//             await DataOrderEntityRepository.SingleAsync(dataEntityCriteria, cancellationToken);
//         return Mapper.Map<TAggregateRoot>(entity);
//     }
//
//     public IQueryable<TAggregateRoot> GetBy(ISpecification<TAggregateRoot> specification)
//     {
//         var dataEntitySpecification = Mapper.Map<ISpecification<TDataEntity>>(specification);
//         var entities = DataOrderEntityRepository.GetBy(dataEntitySpecification);
//         return Mapper.ProjectTo<TAggregateRoot>(entities);
//     }
//
//     public IQueryable<TAggregateRoot> GetBy(Expression<Func<TAggregateRoot, bool>> criteria)
//     {
//         var dataEntityCriteria = Mapper.MapExpression<Expression<Func<TDataEntity, bool>>>(criteria);
//         var entities = DataOrderEntityRepository.GetBy(dataEntityCriteria);
//         return Mapper.ProjectTo<TAggregateRoot>(entities);
//     }
//
//
//     public async Task<TAggregateRoot> GetByIdAsync(TId id,
//         CancellationToken cancellationToken = default)
//     {
//         var entity = await DataOrderEntityRepository.GetByIdAsync(id, cancellationToken);
//         return Mapper.Map<TAggregateRoot>(entity);
//     }
//
//
//     public int Count()
//     {
//         return DataOrderEntityRepository.Count();
//     }
//
//     public async Task<int> CountAsync(CancellationToken cancellationToken = default)
//     {
//         return await DataOrderEntityRepository.CountAsync(cancellationToken);
//     }
//
//     public Task<int> CountAsync(ISpecification<TAggregateRoot> specification,
//         CancellationToken cancellationToken = default)
//     {
//         var dataEntitySpecification = Mapper.Map<ISpecification<TDataEntity>>(specification);
//         return DataOrderEntityRepository.CountAsync(dataEntitySpecification, cancellationToken);
//     }
//
//     public Task<int> CountAsync(Expression<Func<TAggregateRoot, bool>> criteria,
//         CancellationToken cancellationToken = default)
//     {
//         var dataEntityCriteria = Mapper.MapExpression<Expression<Func<TDataEntity, bool>>>(criteria);
//         return DataOrderEntityRepository.CountAsync(dataEntityCriteria, cancellationToken);
//     }
//
//     public long LongCount()
//     {
//         return DataOrderEntityRepository.LongCount();
//     }
//
//     public async Task<long> LongCountAsync(CancellationToken cancellationToken = default)
//     {
//         return await DataOrderEntityRepository.LongCountAsync(cancellationToken);
//     }
//
//     public async Task<long> LongCountAsync(ISpecification<TAggregateRoot> specification,
//         CancellationToken cancellationToken = default)
//     {
//         var dataEntitySpecification = Mapper.Map<ISpecification<TDataEntity>>(specification);
//         return await DataOrderEntityRepository.LongCountAsync(dataEntitySpecification, cancellationToken);
//     }
//
//     public Task<long> LongCountAsync(Expression<Func<TAggregateRoot, bool>> criteria,
//         CancellationToken cancellationToken = default)
//     {
//         var dataEntityCriteria = Mapper.MapExpression<Expression<Func<TDataEntity, bool>>>(criteria);
//         return DataOrderEntityRepository.LongCountAsync(dataEntityCriteria, cancellationToken);
//     }
//
//     public virtual async Task<TAggregateRoot> UpdateAsync(TAggregateRoot entity)
//     {
//         Guard.Against.Null(entity, nameof(entity));
//
//         var originalEntity = await DataOrderEntityRepository.GetByIdAsync(entity.Id);
//         var updatedEntity = Mapper.Map(entity, originalEntity);
//         updatedEntity = await DataOrderEntityRepository.UpdateAsync(updatedEntity);
//         return Mapper.Map<TAggregateRoot>(updatedEntity);
//     }
//
//     public virtual async Task UpdateRangeAsync(IEnumerable<TAggregateRoot> entities)
//     {
//         Guard.Against.Null(entities, nameof(entities));
//
//         var updateTasks = new List<Task>();
//         foreach (var entity in entities)
//             updateTasks.Add(UpdateAsync(entity));
//         Task.WaitAll(updateTasks.ToArray());
//     }
//
//     public virtual async Task DeleteAsync(TAggregateRoot entity)
//     {
//         Guard.Against.Null(entity, nameof(entity));
//
//         var dataEntity = Mapper.Map<TDataEntity>(entity);
//         await DataOrderEntityRepository.DeleteAsync(dataEntity);
//     }
//
//     public virtual async Task DeleteRangeAsync(IEnumerable<TAggregateRoot> entities)
//     {
//         Guard.Against.Null(entities, nameof(entities));
//
//         var dataEntities = Mapper.Map<IEnumerable<TDataEntity>>(entities);
//         await DataOrderEntityRepository.DeleteRangeAsync(dataEntities);
//     }
//
//     #endregion
// }

