using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.Identity.Domain.Entities;
using Astrum.Identity.Models;

namespace Astrum.Identity.Application.Contracts
{
    public interface IGitlabMappingService
    {
        public Task<ApplicationUser> GetApplicationUserByGitlabId(long gitlabId);
        public Task<Guid> AddUserFromGitLab(GitlabUser gitlabUser);
    }
}
