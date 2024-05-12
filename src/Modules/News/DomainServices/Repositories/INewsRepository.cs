using Ardalis.Specification;
using Astrum.News.Aggregates;
using Astrum.SharedLib.Application.Contracts.Persistence.Repositories;

namespace Astrum.News.Repositories;

public interface INewsRepository : IEntityRepository<Post, Guid>
{

    public Task<List<Post>> IncludedListBySpecAsync(ISpecification<Post> spec,
        CancellationToken cancellationToken = default);
    public Task<Post> FirstOrDefaultBySpecAsync(ISpecification<Post> spec,
        CancellationToken cancellationToken = default);
}