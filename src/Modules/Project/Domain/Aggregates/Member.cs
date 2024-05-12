using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.SharedLib.Domain.Entities;

namespace Astrum.Projects.Aggregates
{
    public class Member : AggregateRootBase<Guid>
    {
        private Member() { }

        public Member(Guid id)
        {
            Id = id;
        }

        public Guid UserId { get; set; }
        public bool IsManager { get; set; }
        public string Role { get; set; }
        public Guid? ProjectId { get; set; }
    }
}
