using Astrum.News.Aggregates;
using Astrum.SharedLib.Application.Contracts.Persistence.Repositories;

namespace Astrum.News.Repositories;

public interface ICommentsRepository : IEntityRepository<Comment, Guid>
{
}