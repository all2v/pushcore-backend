using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.Account.Features.Profile;
using Astrum.Appeal.ViewModels;

namespace Astrum.Appeal.Application.ViewModels
{
    public class AppealCreatePageData
    {
        public AppealCreatePageData(List<UserProfileSummary> profileSummaries, List<AppealCategoryForm> appealCategories)
        {
            ProfileSummaries = profileSummaries;
            AppealCategories = appealCategories;
        }

        public List<UserProfileSummary> ProfileSummaries { get; set; }
        public List<AppealCategoryForm> AppealCategories { get; set; }
    }
}
