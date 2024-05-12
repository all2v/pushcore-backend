using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.Infrastructure.Integrations.YouTrack.Models.Project;
using Astrum.Logging.Services;
using Astrum.SharedLib.Common.Results;
using Astrum.TrackerProject.Application.ViewModels;

namespace Astrum.TrackerProject.Application.Services
{
    public interface IIssueService
    {
        Task<Result<List<IssueForm>>> GetIssues(string projectId);
        Task<Result<IssueForm>> GetIssue(string issueId);
        
    }
}
