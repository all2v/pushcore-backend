using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.Account.Features.Profile;

namespace Astrum.TrackerProject.Application.ViewModels
{
    public class ExternalUserForm
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public UserProfileSummary UserProfileSummary { get; set; }
    }
}
