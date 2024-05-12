using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.Identity.Models;
using Astrum.Infrastructure.Integrations.YouTrack.Models.Article;
using Astrum.Infrastructure.Integrations.YouTrack.Models.Project;
using Astrum.Infrastructure.Integrations.YouTrack.Services;
using Astrum.Logging.Services;
using Astrum.TrackerProject.Application.ViewModels;
using Astrum.TrackerProject.Domain.Aggregates;
using Astrum.TrackerProject.Domain.Repositories;
using Astrum.TrackerProject.Domain.Specification;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Astrum.TrackerProject.Application.Services
{
    public class SynchronisationService : ISynchronisationService
    {
        private readonly ITrackerProjectRepository<Domain.Aggregates.Project> _projectRepository;
        private readonly ITrackerProjectRepository<Article> _articleRepository;
        private readonly ITrackerProjectRepository<Issue> _issueRepository;
        private readonly ITrackerProjectRepository<ExternalUser> _externalUserRepository;

        private readonly IMapper _mapper;
        private readonly ITaskTrackerService _taskTrackerService;
        private readonly IProjectService _projectService;
        private readonly UserManager<ApplicationUser> _userManager;

        public SynchronisationService(ITrackerProjectRepository<Domain.Aggregates.Project> projectRepository, 
            IMapper mapper, ITaskTrackerService taskTrackerService, ITrackerProjectRepository<Article> articleRepository,
            ITrackerProjectRepository<Issue> issueRepository, IProjectService projectService, 
            ITrackerProjectRepository<ExternalUser> externalUserRepository, UserManager<ApplicationUser> userManager)
        {
            _projectRepository = projectRepository;
            _mapper = mapper;
            _taskTrackerService = taskTrackerService;
            _articleRepository = articleRepository;
            _issueRepository = issueRepository;
            _projectService = projectService;
            _externalUserRepository = externalUserRepository;
            _userManager = userManager;
        }

        public async Task SynchroniseAll()
        {
            //var synchronisationUsers = await _taskTrackerService.GetSynchronisationUserInfo();
            ////var trackerProjects = await _taskTrackerService.GetProjectsInfo();
            ////var taskTrackerArticles = await _taskTrackerService.GetArticlesInfo();
            await SynchroniseUsers();
            await SynchroniseProjects();
            await SynchroniseArticles();
        }

        public async Task SynchroniseUsers()
        {
            var users = await _externalUserRepository.ListAsync();
            var synchronisationUsers = await _taskTrackerService.GetSynchronisationUserInfo();
            var appUsers = _userManager.Users.ToList();
            foreach (var user in synchronisationUsers)
            {
                if(user.Email is null) continue;

                var external = users.FirstOrDefault(u => u.Id == user.RingId);
                if (external != null)
                {
                    external.Email = user.Email;
                    var appUser = appUsers.FirstOrDefault(x => x.Email == user.Email);
                    if (appUser != null)
                    {
                        external.UserName = appUser.UserName;
                    }
                    await _externalUserRepository.UpdateAsync(external);
                }
                else
                {
                    external = new ExternalUser
                    {
                        Id = user.RingId,
                        Email = user.Email
                    };
                    var appUser = appUsers.FirstOrDefault(x => x.Email == user.Email);
                    if (appUser != null)
                    {
                        external.UserName = appUser.UserName;
                    }
                    await _externalUserRepository.AddAsync(external);
                }
                
            }
            
            await _externalUserRepository.UnitOfWork.SaveChangesAsync(ensureAudit: false);
            //await _logService.LogInfo($"Пользователи портала и youtrack синхронизированы");
        }

        //TODO: Attachments
        public async Task SynchroniseProjects()
        {
            var trackerProjects = await _taskTrackerService.GetProjectsInfo();
            var spec = new GetProjectInfoSpecification();
            var projects = await _projectRepository.ListAsync(spec);
            var dbIssues = projects.SelectMany(x => x.Issues).ToList();

            var trackerIssues = trackerProjects.SelectMany(x => x.Issues);
            await DeleteRedurantIssues(dbIssues, trackerIssues);
            await DeleteRedurantProjects(projects, trackerProjects);

            foreach (var project in trackerProjects)
            {
                var existedProject = projects.FirstOrDefault(x => x.Id == project.Id);
                if (existedProject == null)
                {
                    var newProject = _mapper.Map<Domain.Aggregates.Project>(project);
                    await _projectRepository.AddAsync(newProject);
                }
                else
                {
                    //Automapper еблан
                    existedProject.Name = project.Name ?? existedProject.Name;
                    if (!project.Team.Members?.Any() ?? false) 
                        existedProject.Team.Members = project.Team.Members;

                    existedProject.Team.Name = project.Team.Name;
                    existedProject.Description = project.Description;
                    existedProject.IconUrl = project.IconUrl;
                    existedProject.LeaderId = project.Leader.RingId;
                    existedProject.TeamId = project.Team.Id;

                    foreach (var trackerIssue in project.Issues) 
                        await SynchroniseIssue(dbIssues, trackerIssue, projects);
                }
            }

            //_mapper.Map(trackerProjects, projects);
            //await _projectRepository.UpdateRangeAsync(projects);
            
            await _projectRepository.UnitOfWork.SaveChangesAsync(ensureAudit: false);
            //await _logService.LogInfo($"Портал и youtrack синхронизированы");
        }

        public async Task SynchroniseIssues()
        {
            var spec = new GetIssueWithIncludesSpecification();
            var dbIssues = await _issueRepository.ListAsync(spec);
            var trackerIssues = await _taskTrackerService.GetIssuesInfo();

            //await DeleteRedurantIssues(dbIssues, trackerIssues);

            foreach (var issue in trackerIssues)
            {
                await SynchroniseIssue(dbIssues, issue);
            }

            await _issueRepository.UnitOfWork.SaveChangesAsync(ensureAudit: false);
            //await _logService.LogInfo($"Статьи портала и youtrack синхронизированы");
        }

        public async Task SynchroniseArticles()
        {
            var spec = new GetArticlesWithCommentsSpecification();
            var articles = await _articleRepository.ListAsync(spec);
            var taskTrackerArticles = await _taskTrackerService.GetArticlesInfo();
            foreach (var article in taskTrackerArticles)
            {
                var existedArticle = articles.FirstOrDefault(x => x.Id == article.Id);
                
                if (existedArticle == null)
                {
                    if (article.Content == null) continue;
                    var newArticle = _mapper.Map<Domain.Aggregates.Article>(article);
                    await _articleRepository.AddAsync(newArticle);
                }
                else
                {
                    _mapper.Map(article, existedArticle);
                }
            }

            //_mapper.Map(taskTrackerArticles, articles);
            //await _articleRepository.UpdateRangeAsync(articles);
            
            await _articleRepository.UnitOfWork.SaveChangesAsync(ensureAudit: false);
            //await _logService.LogInfo($"Статьи портала и youtrack синхронизированы");
            
        }

        public async Task SynchroniseIssueChanges(TrackerIssue trackerIssue)
        {
            if (trackerIssue.IsNew)
            {
                var newIssue = _mapper.Map<Issue>(trackerIssue);
                await _issueRepository.AddAsync(newIssue);
            }
            else
            {
                var spec = new GetIssueByIdSpecification(trackerIssue.Id);
                var updating = await _issueRepository.FirstOrDefaultAsync(spec);
                if (updating == null)
                {
                    await SynchroniseIssues();
                    return;
                }
                var updated = _mapper.Map<Issue>(trackerIssue);
                await _issueRepository.UpdateAsync(updated);
            }
            await _issueRepository.UnitOfWork.SaveChangesAsync(ensureAudit: false);
        }

        public async Task SynchroniseArticleChanges(TrackerArticle article)
        {
            if (article.IsNew)
            {
                var newArticle = _mapper.Map<Article>(article);
                await _articleRepository.AddAsync(newArticle);
            }
            else
            {
                var spec = new GetArticleByIdSpecification(article.Id);
                var updating = await _articleRepository.FirstOrDefaultAsync(spec);
                if (updating == null)
                {
                    await SynchroniseArticles();
                    return;
                }
                var updated = _mapper.Map<Article>(article);
                await _articleRepository.UpdateAsync(updated);
            }
            await _articleRepository.UnitOfWork.SaveChangesAsync(ensureAudit: false);
        }

        public async Task SynchroniseProjectChanges(Infrastructure.Integrations.YouTrack.Models.Project.TrackerProject project)
        {
            var spec = new GetProjectByIdSpecification(project.Id);
            var updating = await _projectRepository.FirstOrDefaultAsync(spec);
            if (updating == null)
            {
                await SynchroniseProjects();
                return;
            }
            var team = updating.Team;
            _mapper.Map(project, updating);
            //Automapper еблан
            updating.Name = project.Name;
            updating.Team = team;
            await _projectRepository.UpdateAsync(updating);
            await _projectRepository.UnitOfWork.SaveChangesAsync(ensureAudit: false);
        }

        private async Task SynchroniseIssue(IEnumerable<Issue> issues, TrackerIssue issue, 
            IEnumerable<Domain.Aggregates.Project>? projects = null)
        {
            var updated = issues.FirstOrDefault(x => x.Id == issue.Id);
            if (updated == null)
            {
                var newIssue = _mapper.Map<Issue>(issue);
                if (projects == null)
                    await _issueRepository.AddAsync(newIssue);
                else
                {
                    var editedProject = projects.FirstOrDefault(x => x.Id == newIssue.ProjectId);
                    editedProject.Issues.Add(newIssue);
                }
                
            }
            else
            {
                updated.CommentsCount = issue.CommentsCount;
                updated.Description = issue.Description;
                updated.DraftOwnerId = issue.DraftOwner?.RingId;
                updated.IsDraft = issue.IsDraft;
                updated.ReporterId = issue.Reporter?.RingId;
                updated.Summary = issue.Summary;
                updated.UpdaterId = issue.Updater?.RingId;
                updated.Url = issue.Url;
                updated.Updated = issue.UpdatedDate;
                updated.Resolved = issue.ResolvedDate;
                updated.ProjectId = issue.Project.Id;
                updated.State = GetName(issue, "State");
                updated.Priority = GetName(issue, "Priority");
                updated.AssigneeId = GetRingId(issue, "Assignee");
                foreach (var issueComment in issue.Comments)
                {
                    var existedComment = updated.Comments.FirstOrDefault(x => x.Id == issueComment.Id);
                    if (existedComment == null)
                    {
                        var newComment = _mapper.Map<IssueComment>(issueComment);
                        updated.Comments.Add(newComment);
                    }
                    else
                    {
                        existedComment.AuthorId = issueComment.Author.RingId;
                        existedComment.Text = issueComment.Text;
                    }
                }
            }
        }

        private static string? GetRingId(TrackerIssue src, string fieldName)
        {
            return src.CustomFields.FirstOrDefault(x => x.Name.Equals(fieldName))?.Value?.FirstOrDefault()?.RingId;
        }

        private static string? GetName(TrackerIssue src, string fieldName)
        {
            return src.CustomFields.FirstOrDefault(x => x.Name.Equals(fieldName))?.Value?.FirstOrDefault()?.LocalizedName;
        }

        private async Task DeleteRedurantProjects(List<Domain.Aggregates.Project> projects, List<Infrastructure.Integrations.YouTrack.Models.Project.TrackerProject> trackerProjects)
        {
            var redurantProjects = projects.Where(x => !trackerProjects.Select(tracker => tracker.Id).Contains(x.Id))
                .ToList();
            await _projectRepository.DeleteRangeAsync(redurantProjects);
            await _projectRepository.UnitOfWork.SaveChangesAsync(ensureAudit: false);
        }

        private async Task DeleteRedurantIssues(List<Issue> issues, IEnumerable<TrackerIssue> trackerIssues)
        {
            var redurantIssues = issues.Where(x => !trackerIssues.Select(tracker => tracker.Id).Contains(x.Id)).ToList();
            await _issueRepository.DeleteRangeAsync(redurantIssues);
            await _issueRepository.UnitOfWork.SaveChangesAsync(ensureAudit: false);
        }
    }
}
