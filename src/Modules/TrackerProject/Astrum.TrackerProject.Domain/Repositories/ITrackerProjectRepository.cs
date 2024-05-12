using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.SharedLib.Application.Contracts.Persistence.Repositories;
using Astrum.SharedLib.Domain.Interfaces;

namespace Astrum.TrackerProject.Domain.Repositories
{
    public interface ITrackerProjectRepository<TEntity> : IEntityRepository<TEntity, string>
    where TEntity : class, IEntity<string>
    {
    }
}
