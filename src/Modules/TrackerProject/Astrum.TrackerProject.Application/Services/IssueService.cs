using Astrum.Infrastructure.Integrations.YouTrack.Models.Project;
using Astrum.Infrastructure.Integrations.YouTrack.Services;
using Astrum.Logging.Services;
using Astrum.SharedLib.Common.Results;
using Astrum.TrackerProject.Application.ViewModels;
using Astrum.TrackerProject.Domain.Aggregates;
using Astrum.TrackerProject.Domain.Repositories;
using Astrum.TrackerProject.Domain.Specification;
using AutoMapper;

namespace Astrum.TrackerProject.Application.Services
{
    public class IssueService : IIssueService
    {
        private readonly ITrackerProjectRepository<Issue> _repository;
        private readonly IMapper _mapper;
        private readonly ITaskTrackerService _taskTrackerService;
        private readonly IProjectService _projectService;
        private readonly IExternalUserService _externalUserService;

        public IssueService(ITrackerProjectRepository<Issue> repository, 
            IMapper mapper, ITaskTrackerService taskTrackerService, IProjectService projectService, 
            IExternalUserService externalUserService)
        {
            _repository = repository;
            _mapper = mapper;
            _taskTrackerService = taskTrackerService;
            _projectService = projectService;
            _externalUserService = externalUserService;
        }

        public async Task<Result<List<IssueForm>>> GetIssues(string projectId)
        {
            var spec = new GetIssueByProjectIdSpecification(projectId);
            var issues = await _repository.ListAsync(spec);
            if (!issues.Any())
            {
                return Result<List<IssueForm>>.Error("Задачи по заданному проекту не найдены");
            }
            var issueForms = _mapper.Map<List<IssueForm>>(issues);
            var users = await _externalUserService.GetAllUserProfilesAsync();
            foreach (var form in issueForms)
            {
                SetIssueUsers(form, users);
            }
            return Result.Success(issueForms);
        }

        public async Task<Result<IssueForm>> GetIssue(string issueId)
        {
            var spec = new GetIssueByIdSpecification(issueId);
            var issue = await _repository.FirstOrDefaultAsync(spec);
            if (issue == null)
            {
                return Result.Error("Задача по заданному id не найдена");
            }

            var issueForm = _mapper.Map<IssueForm>(issue);
            var users = await _externalUserService.GetAllUserProfilesAsync();
            SetIssueUsers(issueForm, users);
            foreach (var issueFormComment in issueForm.Comments)
            {
                issueFormComment.Author = users.Data.FirstOrDefault(x => issueFormComment.AuthorId == x.Id)?.UserProfileSummary;
            }

            return Result.Success(issueForm);
        }

        private void SetIssueUsers(IssueForm issueForm, List<ExternalUserForm> users)
        {
            if (!string.IsNullOrWhiteSpace(issueForm.DraftOwnerId))
                issueForm.DraftOwner = users.FirstOrDefault(x => issueForm.DraftOwnerId == x.Id)?.UserProfileSummary;
            if (!string.IsNullOrWhiteSpace(issueForm.ReporterId))
                issueForm.Reporter = users.FirstOrDefault(x => issueForm.ReporterId == x.Id)?.UserProfileSummary;
            if (!string.IsNullOrWhiteSpace(issueForm.UpdaterId))
                issueForm.Updater = users.FirstOrDefault(x => issueForm.UpdaterId == x.Id)?.UserProfileSummary;
            if (!string.IsNullOrWhiteSpace(issueForm.AssigneeId))
                issueForm.Assignee = users.FirstOrDefault(x => issueForm.AssigneeId == x.Id)?.UserProfileSummary;
        }
    }
}
