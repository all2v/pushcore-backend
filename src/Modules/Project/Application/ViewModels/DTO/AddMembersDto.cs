using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.Projects.ViewModels.Requests;

namespace Astrum.Projects.ViewModels
{
    public class AddMembersDto
    {
        public Guid ProjectId { get; set; }
        public List<MemberRequest> Members { get; set; }
    }
}
