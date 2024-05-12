using System.Linq.Expressions;
using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Astrum.SharedLib.Application.Contracts.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Sakura.AspNetCore;

namespace Astrum.SharedLib.Persistence.Repositories;

public abstract class EFReadOnlyRepository<TEntity, TId, TDbContext> : IReadOnlyRepository<TEntity, TId>
    where TEntity : class, Domain.Interfaces.IEntity<TId>
    where TDbContext : DbContext, IUnitOfWork
{
    private readonly ISpecificationEvaluator _specificationEvaluator;

    protected EFReadOnlyRepository(TDbContext context, ISpecificationEvaluator? specificationEvaluator = null)
    {
        _specificationEvaluator = specificationEvaluator ?? SpecificationEvaluator.Default;
        Context = context;
    }

    internal DbContext Context { get; }

    #region IReadOnlyRepository<TEntity,TId> Members

    public IQueryable<TEntity> Items => Context.Set<TEntity>();

    public virtual IQueryable<TEntity> GetBy(ISpecification<TEntity> specification)
    {
        return ApplySpecification(specification);
    }

    public virtual IQueryable<TEntity> GetBy(Expression<Func<TEntity, bool>> criteria)
    {
        return GetItems().Where(criteria);
    }

    public virtual Task<TEntity> GetByIdAsync(TId id,
        CancellationToken cancellationToken = default)
    {
        return GetItems().SingleAsync(i => i.Id!.Equals(id), cancellationToken);
    }

    public virtual Task<List<TEntity>> ListAsync(CancellationToken cancellationToken = default)
    {
        return GetItems().ToListAsync(cancellationToken);
    }

    public virtual Task<List<TEntity>> ListAsync(ISpecification<TEntity> specification,
        CancellationToken cancellationToken = default)
    {
        return ApplySpecification(specification).ToListAsync(cancellationToken);
    }

    public virtual async Task<IPagedList<TEntity>> PagedListAsync(int pageIndex, int pageSize, 
        ISpecification<TEntity>? specification = null)
    {
        var entities = specification is null ? GetItems() : ApplySpecification(specification);
        return await entities.ToPagedListAsync(pageSize, pageIndex);
    }

    public virtual Task<List<TEntity>> ListAsync(Expression<Func<TEntity, bool>> criteria,
        CancellationToken cancellationToken = default)
    {
        return GetItems().Where(criteria).ToListAsync(cancellationToken);
    }

    public virtual bool Any(ISpecification<TEntity> specification)
    {
        return ApplySpecification(specification).Any();
    }

    public virtual bool Any(Expression<Func<TEntity, bool>> criteria)
    {
        return GetItems().Any(criteria);
    }

    public virtual Task<bool> AnyAsync(ISpecification<TEntity> specification,
        CancellationToken cancellationToken = default)
    {
        return ApplySpecification(specification, true).AnyAsync(cancellationToken);
    }

    public virtual Task<bool> AnyAsync(Expression<Func<TEntity, bool>> criteria,
        CancellationToken cancellationToken = default)
    {
        return GetItems().AnyAsync(criteria, cancellationToken);
    }


    public virtual Task<TEntity> FirstAsync(CancellationToken cancellationToken = default)
    {
        return GetItems().FirstAsync(cancellationToken);
    }

    public virtual Task<TEntity> FirstAsync(ISpecification<TEntity> specification,
        CancellationToken cancellationToken = default)
    {
        return ApplySpecification(specification).FirstAsync(cancellationToken);
    }

    public virtual Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> criteria,
        CancellationToken cancellationToken = default)
    {
        return GetItems().FirstAsync(criteria, cancellationToken);
    }

    public TEntity? FirstOrDefault()
    {
        return GetItems().FirstOrDefault();
    }

    public virtual TEntity? FirstOrDefault(ISpecification<TEntity> specification)
    {
        return ApplySpecification(specification).FirstOrDefault();
    }

    public virtual TEntity? FirstOrDefault(Expression<Func<TEntity, bool>> criteria)
    {
        return GetItems().FirstOrDefault(criteria);
    }

    public Task<TEntity?> FirstOrDefaultAsync(CancellationToken cancellationToken = default)
    {
        return GetItems().FirstOrDefaultAsync(cancellationToken);
    }

    public virtual Task<TEntity?> FirstOrDefaultAsync(ISpecification<TEntity> specification,
        CancellationToken cancellationToken = default)
    {
        return ApplySpecification(specification).FirstOrDefaultAsync(cancellationToken);
    }

    public virtual Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> criteria,
        CancellationToken cancellationToken = default)
    {
        return GetItems().FirstOrDefaultAsync(criteria, cancellationToken);
    }

    public virtual TEntity Single(ISpecification<TEntity> specification)
    {
        return ApplySpecification(specification).Single();
    }

    public virtual TEntity Single(Expression<Func<TEntity, bool>> criteria)
    {
        return GetItems().Single(criteria);
    }

    public virtual Task<TEntity> SingleAsync(ISpecification<TEntity> specification,
        CancellationToken cancellationToken = default)
    {
        return ApplySpecification(specification).SingleAsync(cancellationToken);
    }

    public virtual Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> criteria,
        CancellationToken cancellationToken = default)
    {
        return GetItems().SingleAsync(criteria, cancellationToken);
    }

    public virtual int Count()
    {
        return GetItems().Count();
    }

    public virtual Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        return GetItems().CountAsync(cancellationToken);
    }

    public virtual Task<int> CountAsync(ISpecification<TEntity> specification,
        CancellationToken cancellationToken = default)
    {
        return ApplySpecification(specification, true).CountAsync(cancellationToken);
    }

    public virtual Task<int> CountAsync(Expression<Func<TEntity, bool>> criteria,
        CancellationToken cancellationToken = default)
    {
        return GetItems().CountAsync(criteria, cancellationToken);
    }

    public virtual Task<long> LongCountAsync(CancellationToken cancellationToken = default)
    {
        return GetItems().LongCountAsync(cancellationToken);
    }

    public virtual Task<long> LongCountAsync(ISpecification<TEntity> specification,
        CancellationToken cancellationToken = default)
    {
        return ApplySpecification(specification, true).LongCountAsync(cancellationToken);
    }

    public virtual Task<long> LongCountAsync(Expression<Func<TEntity, bool>> criteria,
        CancellationToken cancellationToken = default)
    {
        return GetItems().LongCountAsync(criteria, cancellationToken);
    }

    public virtual TEntity GetById(TId id)
    {
        return GetItems().Single(i => i.Id != null && i.Id.Equals(id));
    }

    public virtual long LongCount()
    {
        return GetItems().LongCount();
    }

    #endregion

    /// <summary>
    ///     Filters the entities  of <typeparamref name="TEntity" />, to those that match the encapsulated query logic of the
    ///     <paramref name="specification" />.
    /// </summary>
    /// <param name="specification">The encapsulated query logic.</param>
    /// <param name="evaluateCriteriaOnly"></param>
    /// <returns>The filtered entities as an <see cref="IQueryable{T}" />.</returns>
    protected virtual IQueryable<TEntity> ApplySpecification(ISpecification<TEntity> specification,
        bool evaluateCriteriaOnly = false)
    {
        var inputQuery = GetItems();
        return _specificationEvaluator.GetQuery(inputQuery, specification, evaluateCriteriaOnly);
    }

    /// <summary>
    ///     Filters all entities of <typeparamref name="TEntity" />, that matches the encapsulated query logic of the
    ///     <paramref name="specification" />, from the database.
    ///     <para>
    ///         Projects each entity into a new form, being <typeparamref name="TResult" />.
    ///     </para>
    /// </summary>
    /// <typeparam name="TResult">The type of the value returned by the projection.</typeparam>
    /// <param name="specification">The encapsulated query logic.</param>
    /// <returns>The filtered projected entities as an <see cref="IQueryable{T}" />.</returns>
    protected virtual IQueryable<TResult> ApplySpecification<TResult>(ISpecification<TEntity, TResult> specification)
    {
        var inputQuery = GetItems();
        return _specificationEvaluator.GetQuery(inputQuery, specification);
    }

    protected virtual IQueryable<TEntity> GetItems()
    {
        return Items;
    }
}