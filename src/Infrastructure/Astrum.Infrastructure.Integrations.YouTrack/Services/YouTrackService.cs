using System;
using Astrum.Infrastructure.Integrations.YouTrack.Models;
using Astrum.Infrastructure.Integrations.YouTrack.Models.Article;
using Astrum.Infrastructure.Integrations.YouTrack.Models.Project;
using Astrum.Logging.Entities;
using Astrum.Logging.Services;
using AutoMapper;
using Markdig;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using YouTrackSharp;
using YouTrackSharp.Issues;

namespace Astrum.Infrastructure.Integrations.YouTrack.Services
{
    public class YouTrackService : ITaskTrackerService
    {
        private readonly IIssuesService issuesService;
        private readonly IMapper _mapper;
        //private readonly ITimeTrackingManagementService timeTrackingManagementService;
        //private readonly ITimeTrackingService timeTrackingService;
        private readonly string? adminConnection;
        private readonly string? apiConnection;
        private readonly string? token;
        private readonly Connection connection;
        private readonly ILogHttpService _logHttpService;
        private readonly ITrackerRequestService _trackerRequestService;

        //TODO: убрать конфигурацию в другое место
        public YouTrackService(IConfiguration config, IMapper mapper, ILogHttpService logHttpService, 
            ITrackerRequestService trackerRequestService)
        {
            var sections = config.GetSection("Youtrack").GetChildren().ToList();
            connection = new BearerTokenConnection(sections[1].Value, sections[2].Value);
            adminConnection = sections[0].Value;
            apiConnection = sections[1].Value;
            token = sections[2].Value;
            issuesService = connection.CreateIssuesService();
            _mapper = mapper;
            _logHttpService = logHttpService;
            _trackerRequestService = trackerRequestService;
            //timeTrackingManagementService = connection.CreateTimeTrackingManagementService();
            //timeTrackingService = connection.CreateTimeTrackingService();
        }

        public async Task<List<SynchronisationUser>> GetSynchronisationUserInfo()
        {
            var url = $"{apiConnection}/api/users?fields=id,fullName,email,ringId&$top=250";
            var result = await _trackerRequestService.GetRequest<List<SynchronisationUser>>(url, token);

            //var client = await connection.GetAuthenticatedApiClient();
            //var users = await client.UsersGetAsync(fields: "ringId,email");
            //var result = users.Select(x => new SynchronisationUser(x.RingId, x.Email)).ToList();
            return result;
        }

        public async Task<List<TrackerProject>> GetProjectsInfo()
        {
            var fields = GetProjectFields();
            var url = $"{apiConnection}/api/admin/projects?fields={fields}";
            var trackerProjects = await _trackerRequestService.GetRequest<List<TrackerProject>>(url, token);
            var issuesCount = trackerProjects.SelectMany(x => x.Issues).Count();
            //var issues = await GetIssuesInfo(issuesCount);
            
            foreach (var project in trackerProjects)
            {
                //project.Issues = issues.Where(x => x.Project.Id.Equals(project.Id)).ToList();
                await SetProjectTeam(project);
                foreach (var issue in project.Issues)
                {
                    if (issue.Updated != null)
                        issue.UpdatedDate = new DateTime(issue.Updated.Value);
                    if (issue.Resolved != null)
                        issue.ResolvedDate = new DateTime(issue.Resolved.Value);
                }
            }
            return trackerProjects;
        }

        public async Task<List<TrackerIssue>> GetIssuesInfo(int top = 0)
        {
            var fields = GetIssueFields();
            var customFields = "customFields=assignee&customFields=priority&customFields=state";
            var url = $"{apiConnection}/api/issues?fields={fields}&$top={top}&{customFields}";
            var trackerIssues = await _trackerRequestService.GetRequest<List<TrackerIssue>>(url, token);
            
            return trackerIssues;
        }

        private async Task SetProjectTeam(TrackerProject project)
        {
            var userGroup = await GetUserGroup(project.Team.Id, project.Team.RingId);
            if (userGroup != null)
                project.Team = new TrackerProjectTeam(userGroup.Id, userGroup.Name, userGroup.Users, project.Id);
        }

        public async Task<List<TrackerArticle>> GetArticlesInfo()
        {
            var fields = GetArticleFields();
            var url = $"{apiConnection}/api/articles?fields={fields}";
            var result = await _trackerRequestService.GetRequest<List<TrackerArticle>>(url, token);
            foreach (var article in result)
            {
                if(article.Content != null)
                {
                    var pipeline = new Markdig.MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
                    article.Content = Markdown.ToHtml(article.Content, pipeline);
                }
            }
            //var client = await connection.GetAuthenticatedApiClient();
            //var articles = await client.ArticlesGetAsync(fields: fields);
            //return _mapper.Map<List<TrackerArticle>>(articles.ToList());
            return result;
        }

        private static string GetProjectFields()
        {
            var issueFields = GetIssueFields();
            var basicInfo = "id,name,shortName,description,leader(ringId)";
            var teamFields = "id,name,icon,usersCount,ringId";
            //var tagFields = "id,color,name";
            var fields = $"{basicInfo},archived,fromEmail,replyToEmail,iconUrl,team({teamFields}),issues({issueFields})";
            return fields;
        }

        private static string GetIssueFields()
        {
            var attachmentFields = "id,base64Content";
            var commentFields = "id,text,author(ringId)";
            var customFields = "name,value(localizedName,ringId)";
            var issueFields =
                $"id,ringId,customFields({customFields}),resolved,updated,summary,project(id),description,reporter(ringId),updater(ringId),comments({commentFields})";
            return issueFields;
        }

        //Подробная информация о статье
        private static string GetArticleFields()
        {
            var attachmentFields = "id,name,size,extension,charset,mimeType,metaData,base64Content";
            var commentFields = "id,text,author(ringId)";
            var content = $"summary,content,attachments({attachmentFields})";
            var otherArticles = $"parentArticle(id),childArticles(id)";
            var fields = $"id,title,description,reporter(ringId),visibility,{content},{otherArticles},project(id),comments({commentFields}),hasStar";
            return fields;
        }

        private async Task<TrackerUserGroup> GetUserGroup(string groupId, string ringId)
        {
            var url = $"{adminConnection}/usergroups/{ringId}?fields=id,name,users(id)";
            var auth = new Tuple<string, string>("Authorization", "Bearer " + token);
            var obj = await HttpHelper.GetAsync(url, headersExt: auth);
            var responseString = await obj.HttpResponseMessage.Content.ReadAsStringAsync();

            if (responseString.Contains("error"))
                return null;

            dynamic responseGroup = JsonConvert.DeserializeObject(responseString);
            var userGroups = new TrackerUserGroup
            {
                Id = groupId,
                Name = (string)responseGroup["name"],
                Users = ((JArray)responseGroup.users)?.Select(user => (string)user["id"])?.ToList()
            };

            return userGroups;
        }
    }
}