using Astrum.Core.Application.Contracts.Persistence.Repositories;
using Astrum.Core.Application.Contracts.Persistence.Repositories.V1;
using Astrum.Core.Domain.Interfaces;
using Astrum.Core.Domain.Specifications;
using Astrum.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Astrum.Core.Persistence.Repositories.V1;

public class EFReadOnlyRepository<TEntity, TId, TDbContext> : ReadOnlyRepository<TEntity, TId>
    where TEntity : class, IDataEntity<TId>
    where TDbContext : DbContext, IUnitOfWork
{
    public EFReadOnlyRepository(TDbContext context)
    {
        Context = context;
        ReadOnly = true;
    }

    internal DbContext Context { get; }
    private DbSet<TEntity> _entities => Context.Set<TEntity>();
    public override IQueryable<TEntity> Items => ReadOnly ? _entities.AsNoTracking() : _entities;
    private IQueryable<TEntity> GetItems(bool isTracked) => isTracked ? _entities : _entities.AsNoTracking();
    public override IQueryable<TEntity> GetBy(ISpecification<TEntity> specification)
    {
        return Items.Where(specification.IsSatisfiedBy());
    }

    public override TEntity Find(TId id)
    {
        return Items.FirstOrDefault(i => i.Id.Equals(id));
    }

    public override async Task<TEntity> FindAsync(TId id, CancellationToken cancellationToken = default)
    {
        return await Items.SingleAsync(i => i.Id!.Equals(id), cancellationToken);
    }

    public override Task<List<TEntity>> ListAsync(CancellationToken cancellationToken = default)
    {
        return Items.ToListAsync(cancellationToken);
    }

    public override Task<List<TEntity>> ListAsync(ISpecification<TEntity> specification,
        CancellationToken cancellationToken = default)
    {
        return Items.Where(specification.IsSatisfiedBy()).ToListAsync(cancellationToken);
        // return Items.IsSatisfiedBy(specification).ToListAsync(cancellationToken);
    }

    public override bool Exists(ISpecification<TEntity> specification)
    {
        return Items.Any(specification.IsSatisfiedBy());
    }

    public override Task<bool> ExistsAsync(ISpecification<TEntity> specification,
        CancellationToken cancellationToken = default)
    {
        return Items.AnyAsync(specification.IsSatisfiedBy(), cancellationToken);
    }

    public override Task<TEntity> FirstAsync(CancellationToken cancellationToken)
    {
        return Items.FirstAsync(cancellationToken);
    }

    public override Task<TEntity> FirstAsync(ISpecification<TEntity> specification,
        CancellationToken cancellationToken = default)
    {
        return Items.FirstAsync(specification.IsSatisfiedBy(), cancellationToken);
    }

    public override TEntity? FirstOrDefault(ISpecification<TEntity> specification)
    {
        return Items.FirstOrDefault(specification.IsSatisfiedBy());
    }

    public override Task<TEntity?> FirstOrDefaultAsync(ISpecification<TEntity> specification, bool isTracked = false,
        CancellationToken cancellationToken = default)
    {
        return GetItems(isTracked).FirstOrDefaultAsync(specification.IsSatisfiedBy(), cancellationToken);
    }

    public override TEntity GetSingle(ISpecification<TEntity> specification)
    {
        return Items.Single(specification.IsSatisfiedBy());
    }

    public override async Task<TEntity> GetSingleAsync(ISpecification<TEntity> specification,
        CancellationToken cancellationToken = default)
    {
        return await Items.SingleAsync(specification.IsSatisfiedBy(), cancellationToken);
    }

    public override Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        return Items.CountAsync(cancellationToken);
    }

    public override Task<int> CountAsync(ISpecification<TEntity> specification,
        CancellationToken cancellationToken = default)
    {
        return Items.CountAsync(specification.IsSatisfiedBy(), cancellationToken);
    }

    public override Task<long> LongCountAsync(CancellationToken cancellationToken = default)
    {
        return Items.LongCountAsync(cancellationToken);
    }

    public override Task<long> LongCountAsync(ISpecification<TEntity> specification,
        CancellationToken cancellationToken = default)
    {
        return Items.LongCountAsync(specification.IsSatisfiedBy(), cancellationToken);
    }

    public override Task<TResult[]> Query<TResult>(Func<IQueryable<TEntity>, IQueryable<TResult>> query,
        CancellationToken cancellationToken = default)
    {
        return query(Items).ToArrayAsync(cancellationToken);
    }

    public override IQueryable<TEntity> Specify(ISpecification<TEntity> specification,
        CancellationToken cancellationToken = default)
    {
        return Items.IsSatisfiedBy(specification);
    }
}