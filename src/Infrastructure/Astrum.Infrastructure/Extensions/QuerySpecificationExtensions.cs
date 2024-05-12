using Astrum.Core.Domain.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Astrum.Infrastructure.Extensions;

public static class QuerySpecificationExtensions
{
    public static IQueryable<T> IsSatisfiedBy<T>(this IQueryable<T> query, ISpecification<T> specification) where T : class
    {
        // fetch a Queryable that includes all expression-based includes
        var queryableResultWithIncludes = specification.Includes
            .Aggregate(query,
                (current, include) => current.Include(include));

        // modify the IQueryable to include any string-based include statements
        var secondaryResult = specification.IncludeStrings
            .Aggregate(queryableResultWithIncludes,
                (current, include) => current.Include(include));

        // return the result of the query using the specification's criteria expression
        return secondaryResult.Where(specification.Criteria);
    }
}