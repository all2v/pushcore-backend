using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Specification;
using Astrum.SharedLib.Persistence.Repositories;
using Astrum.News.Aggregates;

namespace Astrum.News.Repositories
{
    public class WidgetRepository : EFRepository<Widget, Guid, NewsDbContext>, IWidgetRepository
    {
        public WidgetRepository(NewsDbContext context, ISpecificationEvaluator? specificationEvaluator = null) 
            : base(context, specificationEvaluator) 
        { 
        }
    }
}
