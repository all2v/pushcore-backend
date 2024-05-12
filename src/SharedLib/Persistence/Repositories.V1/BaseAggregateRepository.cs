using System.Linq.Expressions;
using Ardalis.GuardClauses;
using Astrum.Core.Application.Contracts.Persistence.Repositories.V1;
using Astrum.Core.Application.Repositories;
using Astrum.Core.Domain.Interfaces;
using Astrum.Core.Domain.Specifications;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using IUnitOfWork = Astrum.Core.Application.Contracts.Persistence.Repositories.IUnitOfWork;

namespace Astrum.Core.Persistence.Repositories.V1;

/// <inheritdoc cref="IBaseAggregateRepository{TAggregateRoot,TDataEntity,TId}" />
/// <summary>
///     Repository implementation of <see cref="IBaseAggregateRepository{TAggregateRoot,TDataEntity,TId}" />.
///     Operates on <see cref="IAggregateRoot{TId}" /> by having the underlying data entity of
///     <see cref="IDataEntity{TId}" />
/// </summary>
public abstract class BaseAggregateRepository<TAggregateRoot, TDataEntity, TId> : IBaseAggregateRepository<
    TAggregateRoot,
    TDataEntity, TId>
    where TAggregateRoot : class, IAggregateRoot<TId>
    where TDataEntity : class, IDataEntity<TId>
{
    /// <summary>
    /// </summary>
    protected readonly IDataEntityRepository<TDataEntity, TId> DataOrderEntityRepository;

    /// <summary>
    /// </summary>
    protected readonly IMapper Mapper;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="mapper"></param>
    /// <param name="dataOrderEntityRepository"></param>
    protected BaseAggregateRepository(IMapper mapper, IDataEntityRepository<TDataEntity, TId> dataOrderEntityRepository)
    {
        Mapper = mapper;
        DataOrderEntityRepository = dataOrderEntityRepository;
        ReadOnly = true;
    }

    #region IBaseAggregateRepository<TAggregateRoot,TDataEntity,TId> Members

    /// <inheritdoc />
    public abstract TDataEntity ToDataEntity(TAggregateRoot aggregateRoot);

    /// <inheritdoc />
    public abstract TAggregateRoot ToAggregateRoot(TDataEntity entity);

    /// <inheritdoc />
    public IUnitOfWork UnitOfWork => DataOrderEntityRepository.UnitOfWork;

    /// <inheritdoc />
    public virtual TAggregateRoot Add(TAggregateRoot entity)
    {
        Guard.Against.Null(entity, nameof(entity));

        var dataEntity = Mapper.Map<TDataEntity>(entity);
        dataEntity = DataOrderEntityRepository.Add(dataEntity);
        return Mapper.Map<TAggregateRoot>(dataEntity);
    }

    /// <inheritdoc />
    public virtual async ValueTask<TAggregateRoot> AddAsync(TAggregateRoot entity,
        CancellationToken cancellationToken = default)
    {
        Guard.Against.Null(entity, nameof(entity));

        var dataEntity = Mapper.Map<TDataEntity>(entity);
        dataEntity = await DataOrderEntityRepository.AddAsync(dataEntity, cancellationToken);
        return Mapper.Map(dataEntity, entity);
    }

    /// <inheritdoc />
    public virtual void AddRange(IEnumerable<TAggregateRoot> entities)
    {
        var dataEntities = Mapper.Map<IEnumerable<TDataEntity>>(entities);
        DataOrderEntityRepository.AddRange(dataEntities);
    }

    /// <inheritdoc />
    public async Task AddRangeAsync(IEnumerable<TAggregateRoot> entities, CancellationToken cancellationToken = default)
    {
        var dataEntities = Mapper.Map<IEnumerable<TDataEntity>>(entities);
        await DataOrderEntityRepository.AddRangeAsync(dataEntities, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<List<TAggregateRoot>> ListAsync(ISpecification<TAggregateRoot> specification,
        CancellationToken cancellationToken = default)
    {
        var dataEntitySpecification = Mapper.Map<ISpecification<TDataEntity>>(specification);
        var entityList = await DataOrderEntityRepository.ListAsync(dataEntitySpecification, cancellationToken);
        return Mapper.Map<List<TAggregateRoot>>(entityList);
    }

    public bool Exists(ISpecification<TAggregateRoot> specification)
    {
        var dataEntitySpecification = Mapper.Map<ISpecification<TDataEntity>>(specification);
        return DataOrderEntityRepository.Exists(dataEntitySpecification);
    }

    /// <inheritdoc />
    public async Task<bool> ExistsAsync(ISpecification<TAggregateRoot> specification,
        CancellationToken cancellationToken = default)
    {
        var dataEntitySpecification = Mapper.Map<ISpecification<TDataEntity>>(specification);
        return await DataOrderEntityRepository.ExistsAsync(dataEntitySpecification, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<TAggregateRoot> FirstAsync(CancellationToken cancellationToken = default)
    {
        var entity = await DataOrderEntityRepository.FirstAsync(cancellationToken);
        return Mapper.Map<TAggregateRoot>(entity);
    }

    /// <inheritdoc />
    public async Task<TAggregateRoot> FirstAsync(ISpecification<TAggregateRoot> specification,
        CancellationToken cancellationToken = default)
    {
        var dataEntitySpecification = Mapper.Map<ISpecification<TDataEntity>>(specification);
        var entity = await DataOrderEntityRepository.FirstAsync(dataEntitySpecification, cancellationToken);
        return Mapper.Map<TAggregateRoot>(entity);
    }

    /// <inheritdoc />
    public TAggregateRoot? FirstOrDefault(ISpecification<TAggregateRoot> specification)
    {
        var dataEntitySpecification = Mapper.Map<ISpecification<TDataEntity>>(specification);
        var entity = DataOrderEntityRepository.FirstOrDefault(dataEntitySpecification);
        return Mapper.Map<TAggregateRoot>(entity);
    }

    /// <inheritdoc />
    public async Task<TAggregateRoot?> FirstOrDefaultAsync(ISpecification<TAggregateRoot> specification, bool isTracked = false,
        CancellationToken cancellationToken = default)
    {
        var dataEntitySpecification = Mapper.Map<ISpecification<TDataEntity>>(specification);
        var entity = await DataOrderEntityRepository.FirstOrDefaultAsync(dataEntitySpecification, isTracked, cancellationToken);
        return Mapper.Map<TAggregateRoot>(entity);
    }

    /// <inheritdoc />
    public TAggregateRoot GetSingle(ISpecification<TAggregateRoot> specification)
    {
        var dataEntitySpecification = Mapper.Map<ISpecification<TDataEntity>>(specification);
        var entity = DataOrderEntityRepository.GetSingle(dataEntitySpecification);
        return Mapper.Map<TAggregateRoot>(entity);
    }

    /// <inheritdoc />
    public async Task<TAggregateRoot> GetSingleAsync(ISpecification<TAggregateRoot> specification,
        CancellationToken cancellationToken = default)
    {
        var dataEntitySpecification = Mapper.Map<ISpecification<TDataEntity>>(specification);
        var entity = await DataOrderEntityRepository.GetSingleAsync(dataEntitySpecification, cancellationToken);
        return Mapper.Map<TAggregateRoot>(entity);
    }

    /// <inheritdoc />
    public bool ReadOnly { get; }

    /// <inheritdoc />
    public IQueryable<TAggregateRoot> Items => Mapper.ProjectTo<TAggregateRoot>(ReadOnly ? DataOrderEntityRepository.Items.AsNoTracking() : DataOrderEntityRepository.Items);

    /// <inheritdoc />
    public IQueryable<TAggregateRoot> GetBy(ISpecification<TAggregateRoot> specification)
    {
        var dataEntitySpecification = Mapper.Map<ISpecification<TDataEntity>>(specification);
        var entities = DataOrderEntityRepository.GetBy(dataEntitySpecification);
        return Mapper.ProjectTo<TAggregateRoot>(entities);
    }

    /// <inheritdoc />
    public TAggregateRoot Find(TId id)
    {
        var entity = DataOrderEntityRepository.Find(id);
        return Mapper.Map<TAggregateRoot>(entity);
    }

    /// <inheritdoc />
    public async Task<TAggregateRoot> FindAsync(TId id, CancellationToken cancellationToken = default)
    {
        var entity = await DataOrderEntityRepository.FindAsync(id, cancellationToken);
        return Mapper.Map<TAggregateRoot>(entity);
    }

    /// <inheritdoc />
    public async Task<List<TAggregateRoot>> ListAsync(CancellationToken cancellationToken = default)
    {
        var entityList = await DataOrderEntityRepository.ListAsync(cancellationToken);
        return Mapper.Map<List<TAggregateRoot>>(entityList);
    }

    /// <inheritdoc />
    public IQueryable<TAggregateRoot> Specify(ISpecification<TAggregateRoot> specification,
        CancellationToken cancellationToken = default)
    {
        var dataEntitySpecification = Mapper.Map<ISpecification<TDataEntity>>(specification);
        var query = DataOrderEntityRepository.Specify(dataEntitySpecification, cancellationToken);
        return Mapper.ProjectTo<TAggregateRoot> (query);
    }

    /// <inheritdoc />
    public async Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        return await DataOrderEntityRepository.CountAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<int> CountAsync(ISpecification<TAggregateRoot> specification,
        CancellationToken cancellationToken = default)
    {
        var expression = Mapper.Map<Expression<Func<TDataEntity, bool>>>(specification.IsSatisfiedBy());
        return await DataOrderEntityRepository.CountAsync(new Specification<TDataEntity>(expression),
            cancellationToken);
    }

    /// <inheritdoc />
    public async Task<long> LongCountAsync(CancellationToken cancellationToken = default)
    {
        return await DataOrderEntityRepository.LongCountAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<long> LongCountAsync(ISpecification<TAggregateRoot> specification,
        CancellationToken cancellationToken = default)
    {
        var dataEntitySpecification = Mapper.Map<ISpecification<TDataEntity>>(specification);
        return await DataOrderEntityRepository.LongCountAsync(dataEntitySpecification, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<TResult[]> Query<TResult>(Func<IQueryable<TAggregateRoot>, IQueryable<TResult>> query,
        CancellationToken cancellationToken = default)
    {
        var expression = Mapper.Map<Expression<Func<IQueryable<TDataEntity>, IQueryable<TResult>>>>(query);
        return await DataOrderEntityRepository.Query(expression.Compile(), cancellationToken);
    }

    /// <inheritdoc />
    public virtual TAggregateRoot Update(TAggregateRoot entity)
    {
        Guard.Against.Null(entity, nameof(entity));

        var originalEntity = DataOrderEntityRepository.Find(entity.Id);
        var updatedEntity = Mapper.Map(entity, originalEntity);
        updatedEntity = DataOrderEntityRepository.Update(updatedEntity);
        return Mapper.Map<TAggregateRoot>(updatedEntity);
    }

    /// <inheritdoc />
    public virtual void UpdateRange(IEnumerable<TAggregateRoot> entities)
    {
        Guard.Against.Null(entities, nameof(entities));

        foreach (var entity in entities) Update(entity);
    }

    /// <inheritdoc />
    public virtual void Delete(TAggregateRoot entity)
    {
        Guard.Against.Null(entity, nameof(entity));

        var dataEntity = Mapper.Map<TDataEntity>(entity);
        DataOrderEntityRepository.Delete(dataEntity);
    }

    /// <inheritdoc />
    public virtual void DeleteRange(IEnumerable<TAggregateRoot> entities)
    {
        Guard.Against.Null(entities, nameof(entities));

        var dataEntities = Mapper.Map<IEnumerable<TDataEntity>>(entities);
        DataOrderEntityRepository.DeleteRange(dataEntities);
    }

    #endregion
}