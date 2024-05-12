using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.Infrastructure.Integrations.YouTrack.Models.Project;
using Astrum.Infrastructure.Shared.Result.AspNetCore;
using Astrum.Infrastructure.Shared;
using Astrum.SharedLib.Common.Results;
using Astrum.TrackerProject.Application.Services;
using Astrum.TrackerProject.Application.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Astrum.Logging.Entities;
using Astrum.Logging.Services;

namespace Astrum.TrackerProject.Backoffice.Controllers
{
    [Area("TrackerProject")]
    [Route("[area]/[controller]")]
    public class TrackerIssueController : ApiBaseController
    {
        private readonly IIssueService _issueService;
        private readonly ISynchronisationService _synchronisationService;
        private readonly ILogHttpService _logHttpService;

        public TrackerIssueController(IIssueService issueService, ISynchronisationService synchronisationService, 
            ILogHttpService logHttpService)
        {
            _issueService = issueService;
            _synchronisationService = synchronisationService;
            _logHttpService = logHttpService;
        }

        /// <summary>
        ///     Получение задач проекта
        /// </summary>
        /// /// <param name="projectId">Id проекта</param>
        /// <returns></returns>
        [HttpGet("issues/{projectId}")]
        [TranslateResultToActionResult]
        [ProducesDefaultResponseType(typeof(Result))]
        [ProducesResponseType(typeof(IssueForm), StatusCodes.Status200OK)]
        public async Task<Result<List<IssueForm>>> GetIssues([FromRoute] string projectId)
        {
            var issues = await _issueService.GetIssues(projectId);
            if (!issues.IsSuccess)
            {
                _logHttpService.Log(null, issues, HttpContext, "Получение задач", TypeRequest.GET,
                    ModuleAstrum.TrackerProject);
            }
            return Result.Success(issues);
        }

        /// <summary>
        ///     Получение конкретной задачи проекта
        /// </summary>
        /// <param name="id">Id задачи</param>
        /// <returns></returns>
        [HttpGet("issue/{id}")]
        [TranslateResultToActionResult]
        [ProducesDefaultResponseType(typeof(Result))]
        [ProducesResponseType(typeof(IssueForm), StatusCodes.Status200OK)]
        public async Task<Result<IssueForm>> GetIssue([FromRoute] string id)
        {
            var issue = await _issueService.GetIssue(id);
            if (!issue.IsSuccess)
            {
                _logHttpService.Log(null, issue, HttpContext, "Получение задач", TypeRequest.GET,
                    ModuleAstrum.TrackerProject);
            }
            return Result.Success(issue);
        }

        [HttpPost("synchroniseissues")]
        [TranslateResultToActionResult]
        [ProducesDefaultResponseType(typeof(Result))]
        [ProducesResponseType(typeof(IssueForm), StatusCodes.Status200OK)]
        public async Task<Result> SynchroniseIssues(TrackerIssue issue)
        {
            await _synchronisationService.SynchroniseIssueChanges(issue);
            return Result.Success();
        }
    }
}
