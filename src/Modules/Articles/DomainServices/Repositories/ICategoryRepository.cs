using Astrum.Articles.Aggregates;
using Astrum.SharedLib.Application.Contracts.Persistence.Repositories;

namespace Astrum.Articles.Repositories
{
    public interface ICategoryRepository : IEntityRepository<Category, Guid>
    {
    }
}
