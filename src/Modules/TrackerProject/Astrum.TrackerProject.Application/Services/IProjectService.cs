using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.Infrastructure.Integrations.YouTrack.Models.Project;
using Astrum.SharedLib.Common.Results;
using Astrum.TrackerProject.Application.ViewModels;

namespace Astrum.TrackerProject.Application.Services
{
    public interface IProjectService
    {
        Task<Result<List<ProjectForm>>> GetProjects(string username);
        Task<Result<ProjectForm>> GetProject(string id);
        //Task<ProjectSynchroniseRequest> GetProjectByYoutrackId(string youtrackId);
    }
}
