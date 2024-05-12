using Ardalis.GuardClauses;
using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Astrum.SharedLib.Application.Contracts.Persistence.Repositories;
using Astrum.SharedLib.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Astrum.ITDictionary.Repositories;

public abstract class BaseRepository<TEntity, TDbContext> : IRepository<TEntity>
    where TEntity : class
    where TDbContext : DbContext, IUnitOfWork
{
    private readonly ISpecificationEvaluator _specificationEvaluator;

    protected BaseRepository(TDbContext context, ISpecificationEvaluator? specificationEvaluator = null)
    {
        _specificationEvaluator = specificationEvaluator ?? SpecificationEvaluator.Default;
        Context = context;
        UnitOfWork = context;
    }

    public IUnitOfWork UnitOfWork { get; }

    private TDbContext Context { get; }

    public TEntity Add(TEntity entity)
    {
        Guard.Against.Null(entity);
        Entities.Add(entity);
        var entry = Entities.Add(entity);
        return entry.Entity;
    }

    public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        Guard.Against.Null(entity);
        var entry = await Entities.AddAsync(entity, cancellationToken);
        return entry.Entity;
    }

    public void AddRange(IEnumerable<TEntity> entities)
    {
        Entities.AddRange(entities);
    }

    public async Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        await Entities.AddRangeAsync(entities, cancellationToken);
    }

    public Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        Guard.Against.Null(entity);
        Entities.Remove(entity);
        return Task.CompletedTask;
    }

    public Task DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        Guard.Against.Null(entities);
        Entities.RemoveRange(entities);
        return Task.CompletedTask;
    }

    public Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        Guard.Against.Null(entity);
        var entry = Entities.Update(entity);
        return Task.FromResult(entry.Entity);
    }

    public Task UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        Guard.Against.Null(entities);
        var updateTasks = new List<Task>();
        foreach (var entity in entities)
            updateTasks.Add(UpdateAsync(entity, cancellationToken));
        Task.WaitAll(updateTasks.ToArray());
        return Task.CompletedTask;
    }

    private DbSet<TEntity> Entities => Context.Set<TEntity>();

    public Task<List<TEntity>> ListAsync(CancellationToken cancellationToken = default)
    {
        return GetItems().ToListAsync(cancellationToken);
    }

    public Task<List<TEntity>> ListAsync(ISpecification<TEntity> specification,
        CancellationToken cancellationToken = default)
    {
        return ApplySpecification(specification).ToListAsync(cancellationToken);
    }

    public Task<bool> AnyAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default)
    {
        return ApplySpecification(specification).AnyAsync(cancellationToken);
    }

    public Task<TEntity> FirstAsync(CancellationToken cancellationToken = default)
    {
        return GetItems().FirstAsync(cancellationToken);
    }

    public Task<TEntity> FirstAsync(ISpecification<TEntity> specification,
        CancellationToken cancellationToken = default)
    {
        return ApplySpecification(specification).FirstAsync(cancellationToken);
    }

    public Task<TEntity?> FirstOrDefaultAsync(CancellationToken cancellationToken = default)
    {
        return GetItems().FirstOrDefaultAsync(cancellationToken);
    }

    public Task<TEntity?> FirstOrDefaultAsync(ISpecification<TEntity> specification,
        CancellationToken cancellationToken = default)
    {
        return ApplySpecification(specification).FirstOrDefaultAsync(cancellationToken);
    }

    public Task<TEntity> SingleAsync(ISpecification<TEntity> specification,
        CancellationToken cancellationToken = default)
    {
        return ApplySpecification(specification).SingleAsync(cancellationToken);
    }

    public Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        return GetItems().CountAsync(cancellationToken);
    }

    public Task<int> CountAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default)
    {
        return ApplySpecification(specification).CountAsync(cancellationToken);
    }

    public long LongCount()
    {
        return GetItems().LongCount();
    }

    public Task<long> LongCountAsync(CancellationToken cancellationToken = default)
    {
        return GetItems().LongCountAsync(cancellationToken);
    }

    public Task<long> LongCountAsync(ISpecification<TEntity> specification,
        CancellationToken cancellationToken = default)
    {
        return ApplySpecification(specification).LongCountAsync(cancellationToken);
    }

    protected virtual IQueryable<TEntity> ApplySpecification(ISpecification<TEntity> specification,
        bool evaluateCriteriaOnly = false)
    {
        var inputQuery = GetItems();
        return _specificationEvaluator.GetQuery(inputQuery, specification, evaluateCriteriaOnly);
    }

    protected virtual IQueryable<TResult> ApplySpecification<TResult>(ISpecification<TEntity, TResult> specification)
    {
        var inputQuery = GetItems();
        return _specificationEvaluator.GetQuery(inputQuery, specification);
    }

    protected virtual IQueryable<TEntity> GetItems()
    {
        var isSafetyRemovableEntity = typeof(TEntity).IsAssignableTo(typeof(ISafetyRemovableEntity));
        // TODO Вероятно это не лучшее место для данной логики!
        return isSafetyRemovableEntity
            ? Entities.Where(x => !(x as ISafetyRemovableEntity)!.IsDeleted)
            : Entities;
    }
}