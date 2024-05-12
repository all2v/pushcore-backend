using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.Appeal.Aggregates;
using Astrum.SharedLib.Domain.Entities;

namespace Astrum.Appeal.Domain.Aggregates
{
    public class AppealAppealCategory : AggregateRootBase<Guid>
    {
        public Guid AppealId { get; set; }
        public Guid AppealCategoryId { get; set; }

        public Appeal.Aggregates.Appeal Appeal { get; set; }
        public AppealCategory Category { get; set; }
    }
}
