using Astrum.ITDictionary.Aggregates;
using Astrum.SharedLib.Application.Contracts.Persistence.Repositories;

namespace Astrum.ITDictionary.Repositories;

public interface ICategoryRepository: IEntityRepository<Category, Guid>
{
    
}