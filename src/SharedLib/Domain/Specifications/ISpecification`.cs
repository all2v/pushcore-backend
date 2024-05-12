using System.Linq.Expressions;

namespace Astrum.Core.Domain.Specifications;

public interface ISpecification<T>
{
    Expression<Func<T, bool>> IsSatisfiedBy();
    Expression<Func<T, bool>> Criteria { get; }
    List<string> IncludeStrings { get; }
    List<Expression<Func<T, object>>> Includes { get; }
}