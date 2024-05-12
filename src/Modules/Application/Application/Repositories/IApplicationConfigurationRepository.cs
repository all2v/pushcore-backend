using Astrum.Application.Aggregates;
using Astrum.SharedLib.Application.Contracts.Persistence.Repositories;

namespace Astrum.Application.Repositories;

public interface IApplicationConfigurationRepository : IEntityRepository<ApplicationConfiguration, string>
{
}