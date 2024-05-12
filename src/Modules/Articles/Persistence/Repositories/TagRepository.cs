using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Specification;
using Astrum.Articles.Aggregates;
using Astrum.Articles.Repositories;
using Astrum.SharedLib.Persistence.Repositories;

namespace Astrum.Articles.Repositories
{
    public class TagRepository : EFRepository<Tag, Guid, ArticlesDbContext>,
    ITagRepository
    {
        public TagRepository(ArticlesDbContext context, ISpecificationEvaluator? specificationEvaluator = null) : base(
            context, specificationEvaluator)
        {
        }
    }
}
