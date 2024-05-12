using Astrum.Infrastructure.Integrations.YouTrack.Models;
using Astrum.Infrastructure.Integrations.YouTrack.Models.Article;
using Astrum.Infrastructure.Integrations.YouTrack.Models.Project;
using YouTrackSharp.Generated;
using Project = YouTrackSharp.Projects.Project;

namespace Astrum.Infrastructure.Integrations.YouTrack.Services
{
    public interface ITaskTrackerService
    {
        Task<List<TrackerProject>> GetProjectsInfo();
        Task<List<TrackerIssue>> GetIssuesInfo(int top = 0);
        Task<List<TrackerArticle>> GetArticlesInfo();
        Task<List<SynchronisationUser>> GetSynchronisationUserInfo();
    }
}

