using Ardalis.GuardClauses;
using Astrum.Core.Application.Contracts.Persistence.Repositories;
using Astrum.Core.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Astrum.Core.Persistence.Repositories.V1;

public abstract class EFRepository<TEntity, TId, TDbContext> : EFReadOnlyRepository<TEntity, TId, TDbContext>, Application.Contracts.Persistence.Repositories.V1.IEntityRepository<TEntity, TId>
    where TEntity : class, IDataEntity<TId>
    where TDbContext : DbContext, IUnitOfWork
{
    private readonly IUnitOfWork _unitOfWork;

    public EFRepository(TDbContext context) : base(context)
    {
        Context = context;
        _unitOfWork = context;
        ReadOnly = false;
    }

    internal DbContext Context { get; }

    private DbSet<TEntity> _entities => Context.Set<TEntity>();

    #region IEntityRepository<TEntity,TId> Members

    public IUnitOfWork UnitOfWork
    {
        get
        {
            if (ReadOnly) throw new NotImplementedException("For current repository enabled readonly mode");

            return _unitOfWork;
        }
    }

    public override IQueryable<TEntity> Items => ReadOnly ? _entities.AsNoTracking() : _entities;


    public TEntity Add(TEntity entity)
    {
        Guard.Against.Null(entity, nameof(entity));
        _entities.Add(entity);
        var entry = _entities.Add(entity);
        return entry.Entity;
    }

    public async ValueTask<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken)
    {
        Guard.Against.Null(entity, nameof(entity));
        var entry = await _entities.AddAsync(entity, cancellationToken);
        return entry.Entity;
    }

    public void AddRange(IEnumerable<TEntity> entities)
    {
        _entities.AddRange(entities);
    }

    public async Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
    {
        await _entities.AddRangeAsync(entities, cancellationToken);
    }

    public void Delete(TEntity entity)
    {
        Guard.Against.Null(entity, nameof(entity));
        _entities.Remove(entity);
    }

    public void DeleteRange(IEnumerable<TEntity> entities)
    {
        Guard.Against.Null(entities, nameof(entities));
        _entities.RemoveRange(entities);
    }

    public TEntity Update(TEntity entity)
    {
        Guard.Against.Null(entity, nameof(entity));
        var entry = _entities.Update(entity); // TODO unsuported wtf?
        return entry.Entity;
    }

    public void UpdateRange(IEnumerable<TEntity> entities)
    {
        Guard.Against.Null(entities, nameof(entities));
        foreach (var entity in entities)
            Update(entity);
    }

    #endregion
}