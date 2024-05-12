using Mono.Linq.Expressions;

namespace Astrum.Core.Domain.Specifications;

public static class SpecificationExtensions
{
    public static ISpecification<TEntity> And<TEntity>(this ISpecification<TEntity> specLeft,
        ISpecification<TEntity> specRight)
    {
        if (specLeft == null) return specRight;

        var specLeftExpresion = specLeft.IsSatisfiedBy();
        var specRightExpression = specRight.IsSatisfiedBy();

        var andAlsoExpression =
            specLeftExpresion.AndAlso(specRightExpression);

        return new Specification<TEntity>(andAlsoExpression);
    }

    public static ISpecification<TEntity> Or<TEntity>(this ISpecification<TEntity> specLeft,
        ISpecification<TEntity> specRight)
    {
        if (specLeft == null) return specRight;

        var specLeftExpresion = specLeft.IsSatisfiedBy();
        var specRightExpression = specRight.IsSatisfiedBy();

        var orExpression = specLeftExpresion.OrElse(specRightExpression);
        return new Specification<TEntity>(orExpression);
    }

    public static ISpecification<TEntity> Not<TEntity>(ISpecification<TEntity> specification)
    {
        var notExpression = specification.IsSatisfiedBy().Not();
        return new Specification<TEntity>(notExpression);
    }
}