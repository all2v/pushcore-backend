using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.Infrastructure.Shared.Result.AspNetCore;
using Astrum.Infrastructure.Shared;
using Astrum.Logging.Services;
using Astrum.SharedLib.Common.Results;
using Astrum.TrackerProject.Application.Services;
using Astrum.TrackerProject.Application.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Astrum.Logging.Entities;
using Astrum.Infrastructure.Integrations.YouTrack.Models.Project;

namespace Astrum.TrackerProject.Backoffice.Controllers
{

    [Area("TrackerProject")]
    [Route("[area]/[controller]")]
    public class TrackerProjectController : ApiBaseController
    {
        private readonly IProjectService _projectService;
        private readonly ILogHttpService _logHttpService;
        private readonly ISynchronisationService _synchronisationService;

        public TrackerProjectController(IProjectService projectService, ILogHttpService logHttpService, 
            ISynchronisationService synchronisationService)
        {
            _projectService = projectService;
            _logHttpService = logHttpService;
            _synchronisationService = synchronisationService;
        }

        /// <summary>
        ///     Получение проектов из youtrack
        /// </summary>
        /// <returns></returns>
        [HttpGet("user/{username}")]
        [TranslateResultToActionResult]
        [ProducesDefaultResponseType(typeof(Result))]
        [ProducesResponseType(typeof(ProjectForm), StatusCodes.Status200OK)]
        public async Task<Result<List<ProjectForm>>> GetProjects([FromRoute] string username)
        {
            var projects = await _projectService.GetProjects(username);
            if (!projects.IsSuccess)
            {
                _logHttpService.Log(null, projects, HttpContext, "Получение проектов", TypeRequest.GET,
                    ModuleAstrum.TrackerProject);
            }
            return Result.Success(projects);
        }

        /// <summary>
        ///     Получение проекта из youtrack
        /// </summary>
        /// <param name="id">Id проекта</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [TranslateResultToActionResult]
        [ProducesDefaultResponseType(typeof(Result))]
        [ProducesResponseType(typeof(ProjectForm), StatusCodes.Status200OK)]
        public async Task<Result<ProjectForm>> GetProject([FromRoute] string id)
        {
            var project = await _projectService.GetProject(id);
            if (!project.IsSuccess)
            {
                _logHttpService.Log(null, project, HttpContext, "Получение проекта", TypeRequest.GET,
                    ModuleAstrum.TrackerProject);
            }
            return Result.Success(project);
        }

        [HttpPost("synchroniseprojects")]
        [TranslateResultToActionResult]
        [ProducesDefaultResponseType(typeof(Result))]
        [ProducesResponseType(typeof(ProjectForm), StatusCodes.Status200OK)]
        public async Task<Result> SynchroniseProjects(Infrastructure.Integrations.YouTrack.Models.Project.TrackerProject project)
        {
            await _synchronisationService.SynchroniseProjectChanges(project);
            return Result.Success();
        }
    }
}
