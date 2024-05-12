using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.SharedLib.Domain.Entities;

namespace Astrum.Account.Aggregates
{
    public class CustomField : AggregateRootBase<Guid>
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public Guid UserProfileId { get; set; }
    }
}
