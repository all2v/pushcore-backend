using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Specification;
using Astrum.Projects.Aggregates;

namespace Astrum.Projects.Specifications.Customer
{
    public class GetMembersSpec : Specification<Member>
    {
    }

    public class GetMembersByIdSpec : GetMembersSpec { 
        public GetMembersByIdSpec(Guid id)
        {
            Query
                .Where(x => x.Id == id);
        }
    }

    public class GetByProjectIdSpec : GetMembersSpec
    {
        public GetByProjectIdSpec(Guid id)
        {
            Query
                .Where(x => x.ProjectId == id);
        }
    }
}
