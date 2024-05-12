using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.Infrastructure.Integrations.YouTrack;
using Astrum.Logging.Services;
using Astrum.SharedLib.Common.Results;
using Astrum.TrackerProject.Application.ViewModels;
using Astrum.TrackerProject.Domain.Aggregates;
using Astrum.TrackerProject.Domain.Repositories;
using Astrum.TrackerProject.Domain.Specification;
using AutoMapper;

namespace Astrum.TrackerProject.Application.Services
{
    public class ProjectService : IProjectService
    {
        private readonly ITrackerProjectRepository<Domain.Aggregates.Project> _projectRepository;
        private readonly ITrackerProjectRepository<ExternalUser> _userRepository;
        private readonly ITrackerProjectRepository<Article> _articleRepository;
        private readonly IMapper _mapper;
        private readonly IExternalUserService _externalUserService;

        public ProjectService(ITrackerProjectRepository<Domain.Aggregates.Project> projectRepository, IMapper mapper,
            IExternalUserService externalUserService, 
            ITrackerProjectRepository<ExternalUser> userRepository, ITrackerProjectRepository<Article> articleRepository)
        {
            _projectRepository = projectRepository;
            _mapper = mapper;
            _externalUserService = externalUserService;
            _userRepository = userRepository;
            _articleRepository = articleRepository;
        }

        public async Task<Result<List<ProjectForm>>> GetProjects(string username)
        {
            var userSpec = new GetExternalUserByUsernameSpecification(username);
            var user = await _userRepository.FirstOrDefaultAsync(userSpec);
            if (user == null)
            {
                return Result<List<ProjectForm>>.Error("Пользователь не найден");
            }
            var projectSpec = new GetProjectSpecification();
            var projects = await _projectRepository.ListAsync(projectSpec);
            projects = projects.Where(proj => proj.Team != null && proj.Team.Members.Contains(user.Id)).ToList();
            var projectForms = _mapper.Map<List<ProjectForm>>(projects);
            var users = await _externalUserService.GetAllUserProfilesAsync(true);
            var articles = await _articleRepository.ListAsync();
            foreach (var projectForm in projectForms)
            {
                projectForm.Leader = users.Data.FirstOrDefault(x => projectForm.LeaderId == x.Id)?.UserProfileSummary;
                var projectTeam = projects.FirstOrDefault(x => x.Id == projectForm.Id).Team;
                projectForm.Articles = articles.Count(x => x.ProjectId == projectForm.Id);
                SetProjectTeam(projectTeam, projectForm, users.Data);
            }
            return Result.Success(projectForms);
        }

        public async Task<Result<ProjectForm>> GetProject(string id)
        {
            var spec = new GetProjectByIdSpecification(id);
            var project = await _projectRepository.FirstOrDefaultAsync(spec);
            if (project == null)
            {
                return Result<ProjectForm>.Error("Проект по заданному id не найдена");
            }

            var projectForm = _mapper.Map<ProjectForm>(project);
            var users = await _externalUserService.GetAllUserProfilesAsync(true);
            var articlesSpec = new GetArticleByProjectIdSpecification(projectForm.Id);
            projectForm.Articles = await _articleRepository.CountAsync(articlesSpec);
            projectForm.Leader = users.Data.FirstOrDefault(x => projectForm.LeaderId == x.Id)?.UserProfileSummary;
            SetProjectTeam(project.Team, projectForm, users.Data);

            return Result.Success(projectForm);
        }

        private static void SetProjectTeam(Team team, ProjectForm projectForm, List<ExternalUserForm> users)
        {
            if (team != null)
                projectForm.Members = users
                    .Where(x => x.UserProfileSummary != null && team.Members.Contains(x.Id))
                    .Select(x => x.UserProfileSummary).ToList();
        }

        //public async Task<ProjectSynchroniseRequest> GetProjectByYoutrackId(string youtrackId)
        //{
        //    var spec = new GetProjectByIdSpecification(youtrackId);
        //    var project = await _repository.FirstOrDefaultAsync(spec);
        //    if (project == null)
        //    {
        //        //sentry
        //    }

        //    return _mapper.Map<ProjectSynchroniseRequest>(project);
        //}
    }
}
