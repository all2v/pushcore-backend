using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Specification;
using Astrum.TrackerProject.Domain.Aggregates;

namespace Astrum.TrackerProject.Domain.Specification
{
    public class GetProjectByIdSpecification : Specification<Aggregates.Project>
    {
        public GetProjectByIdSpecification(string id)
        {
            Query
                .Where(x => x.Id == id)
                .Include(project => project.Team);
        }
    }

    public class GetProjectSpecification : Specification<Aggregates.Project>
    {
        public GetProjectSpecification()
        {
            Query
                .Include(project => project.Team);
        }
    }

    public class GetProjectInfoSpecification : Specification<Aggregates.Project>
    {
        public GetProjectInfoSpecification()
        {
            Query
                .Include(project => project.Team)
                .Include(project => project.Issues)
                .ThenInclude(issue => issue.Comments);
        }
    }
}
