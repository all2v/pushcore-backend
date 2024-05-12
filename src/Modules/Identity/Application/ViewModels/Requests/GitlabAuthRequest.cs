using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrum.Identity.Application.ViewModels.Requests
{
    public class GitlabAuthRequest
    {
        public string Code { get; set; }
        public string RedirectUri { get; set; }
    }
}
