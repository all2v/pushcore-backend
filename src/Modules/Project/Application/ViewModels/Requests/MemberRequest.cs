using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrum.Projects.ViewModels.Requests
{
    public class MemberRequest
    {
        public Guid UserId { get; set; }
        public bool IsManager { get; set; }
        public string Role { get; set; }
    }
}
