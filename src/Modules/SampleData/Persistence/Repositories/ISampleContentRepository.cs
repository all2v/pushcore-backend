using Astrum.SampleData.Aggregates;
using Astrum.SharedLib.Application.Contracts.Persistence.Repositories;

namespace Astrum.SampleData.Repositories;
public interface ISampleContentRepository : IEntityRepository<SampleContentFile, Guid>
{

}
