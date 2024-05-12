using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Specification;
using Astrum.TrackerProject.Domain.Aggregates;

namespace Astrum.TrackerProject.Domain.Specification
{
    public class GetIssueByProjectIdSpecification : Specification<Issue>
    {
        public GetIssueByProjectIdSpecification(string projectId)
        {
            Query.Where(x => x.ProjectId == projectId);
        }
    }

    public class GetIssueByIdSpecification : Specification<Issue>
    {
        public GetIssueByIdSpecification(string id)
        {
            Query.Where(x => x.Id == id);
        }
    }

    public class GetIssueWithIncludesSpecification : Specification<Issue>
    {
        public GetIssueWithIncludesSpecification()
        {
            Query.Include(x => x.Comments);
        }
    }
}
