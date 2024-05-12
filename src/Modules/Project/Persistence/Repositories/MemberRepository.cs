using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Specification;
using Astrum.Projects.Aggregates;
using Astrum.SharedLib.Persistence.Repositories;

namespace Astrum.Projects.Repositories
{
    public class MemberRepository : EFRepository<Member, Guid, ProjectDbContext>,
    IMemberRepository
    {
        public MemberRepository(ProjectDbContext context, ISpecificationEvaluator? specificationEvaluator = null) : base(
            context, specificationEvaluator)
        {
        }
    }
}
