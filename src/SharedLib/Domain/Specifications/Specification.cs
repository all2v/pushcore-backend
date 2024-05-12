using System.Linq.Expressions;

namespace Astrum.Core.Domain.Specifications;

public class Specification<TEntity> : ISpecification<TEntity>
{
    public Expression<Func<TEntity, bool>> Criteria { get; set; }
    public List<Expression<Func<TEntity, object>>> Includes { get; } = new List<Expression<Func<TEntity, object>>>();
    public List<string> IncludeStrings { get; } = new List<string>();

    private Specification()
    {
    }

    public Specification(Expression<Func<TEntity, bool>> expression)
    {
        Criteria = expression;
    }

    #region ISpecification<TEntity> Members

    public Expression<Func<TEntity, bool>> IsSatisfiedBy()
    {
        return Criteria;
    }

    #endregion

    public static ISpecification<TEntity> Empty()
    {
        return new Specification<TEntity>(x => true);
    }

    public static ISpecification<TEntity> Create(Expression<Func<TEntity, bool>> expression)
    {
        return new Specification<TEntity>(expression);
    }
}