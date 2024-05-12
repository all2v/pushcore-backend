using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.Identity.Models;

namespace Astrum.Identity.Application.ViewModels
{
    public class GitlabUserCreateResult
    {
        public bool Successful { get; set; }
        public string ErrorMessage { get; set; }
        public ApplicationUser User { get; set; }
    }
}
