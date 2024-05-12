namespace Astrum.Core.Domain.Specifications;

public static class SpecificationLinqExtensions
{
    public static IQueryable<TEntity> Where<TEntity>(this IQueryable<TEntity> query,
        ISpecification<TEntity> specification)
    {
        query = query.Where(specification.IsSatisfiedBy());
        return query;
    }

    public static IEnumerable<TEntity> Where<TEntity>(this IEnumerable<TEntity> query,
        ISpecification<TEntity> specification)
    {
        var compileFunc = specification.IsSatisfiedBy().Compile();
        query = query.Where(compileFunc);
        return query;
    }
}