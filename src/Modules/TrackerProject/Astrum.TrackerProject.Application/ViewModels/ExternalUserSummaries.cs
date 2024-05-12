using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.Account.Features.Profile;
using Sakura.AspNetCore;

namespace Astrum.TrackerProject.Application.ViewModels
{
    public class ExternalUserSummaries
    {
        public List<ExternalUserForm> ExternalUsers { get; set; }
        public List<UserProfileSummary> ProfileSummaries { get; set; }
    }

    public class PagedExternalUserSummaries
    {
        public IPagedList<ExternalUserForm> ExternalUsers { get; set; }
        public List<UserProfileSummary> ProfileSummaries { get; set; }
    }
}
