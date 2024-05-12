using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrum.Projects.ViewModels.Requests
{
    public class ProjectRequest
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public Guid? ProductId { get; set; }
        public List<MemberRequest>? Members { get; set; }
        public List<CustomFieldRequest>? CustomFields { get; set; }
    }
}
