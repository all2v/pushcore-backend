using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.SharedLib.Domain.Entities;

namespace Astrum.TrackerProject.Domain.Aggregates
{
    public class Team : AggregateRootBase<string>
    {
        public string Name { get; set; }
        public string ProjectId { get; set; }
        [Column(TypeName = "jsonb")]
        public List<string> Members { get; set; }
    }
}
