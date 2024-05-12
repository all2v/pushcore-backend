using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Specification;

namespace Astrum.Account.Specifications.UserProfile
{
    public class GetTimelineByUserIdSpecification : Specification<Aggregates.AccessTimeline>
    {
        public GetTimelineByUserIdSpecification(Guid id)
        {
            Query.Where(x => x.UserId == id).Include(x => x.Intervals);
        }
    }
}
