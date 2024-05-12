using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.Infrastructure.Integrations.YouTrack.Models.Article;
using Astrum.Infrastructure.Integrations.YouTrack.Models.Project;
using Astrum.TrackerProject.Application.ViewModels;

namespace Astrum.TrackerProject.Application.Services
{
    public interface ISynchronisationService
    {
        Task SynchroniseAll();
        Task SynchroniseProjects();
        Task SynchroniseProjectChanges(Infrastructure.Integrations.YouTrack.Models.Project.TrackerProject project);
        Task SynchroniseIssues();
        Task SynchroniseIssueChanges(TrackerIssue trackerIssue);
        Task SynchroniseArticles();
        Task SynchroniseArticleChanges(TrackerArticle article);
    }
}
