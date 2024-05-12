using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.Account.Aggregates;
using Astrum.SharedLib.Domain.Entities;

namespace Astrum.Account.Domain.Aggregates
{
    public class Position : AggregateRootBase<Guid>
    {
        public string Name { get; set; }
        public IEnumerable<UserProfile> UserProfiles { get; set; }
    }
}
