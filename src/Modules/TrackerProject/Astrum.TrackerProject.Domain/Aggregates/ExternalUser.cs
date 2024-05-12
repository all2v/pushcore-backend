using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.SharedLib.Domain.Entities;

namespace Astrum.TrackerProject.Domain.Aggregates
{
    public class ExternalUser : AggregateRootBase<string>
    {
        public string? UserName { get; set; }
        public string Email { get; set; }
    }
}
