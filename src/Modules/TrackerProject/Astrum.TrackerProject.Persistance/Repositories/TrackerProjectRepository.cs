using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Specification;
using Astrum.SharedLib.Persistence.Repositories;
using Astrum.TrackerProject.Domain.Repositories;

namespace Astrum.TrackerProject.Persistance.Repositories
{
    public class TrackerProjectRepository<TEntity> : EFRepository<TEntity, string, TrackerProjectDbContext>,
        ITrackerProjectRepository<TEntity> where TEntity : class, SharedLib.Domain.Interfaces.IEntity<string>
    {
        public TrackerProjectRepository(TrackerProjectDbContext context, ISpecificationEvaluator? specificationEvaluator = null) : base(context, specificationEvaluator)
        {
        }
    }
}
