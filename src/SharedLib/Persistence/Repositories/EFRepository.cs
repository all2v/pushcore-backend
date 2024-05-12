using Ardalis.GuardClauses;
using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Astrum.SharedLib.Application.Contracts.Persistence.Repositories;
using Astrum.SharedLib.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Astrum.SharedLib.Persistence.Repositories;

public abstract class EFRepository<TEntity, TId, TDbContext> : EFReadOnlyRepository<TEntity, TId, TDbContext>
    , IEntityRepository<TEntity, TId>
    where TEntity : class, Domain.Interfaces.IEntity<TId>
    where TDbContext : DbContext, IUnitOfWork
{
    protected EFRepository(TDbContext context, ISpecificationEvaluator? specificationEvaluator = null)
        : base(context, specificationEvaluator ?? SpecificationEvaluator.Default)
    {
        UnitOfWork = context;
    }

    private DbSet<TEntity> Entities => Context.Set<TEntity>();

    #region IEntityRepository<TEntity,TId> Members

    public IUnitOfWork UnitOfWork { get; }

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
        var entry = Entities.Update(entity); // TODO unsuported wtf?
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

    #endregion

    protected override IQueryable<TEntity> GetItems()
    {
        var isSafetyRemovableEntity = typeof(TEntity).IsAssignableTo(typeof(ISafetyRemovableEntity));
        // TODO Вероятно это не лучшее место для данной логики!
        return isSafetyRemovableEntity
            ? Entities.Where(x => !(x as ISafetyRemovableEntity)!.IsDeleted)
            : Entities;
    }
}