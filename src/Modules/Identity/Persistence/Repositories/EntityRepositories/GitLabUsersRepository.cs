using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.Identity.Application.Repositories;
using Astrum.Identity.Domain.Entities;
using Astrum.Identity.Models;
using Astrum.Identity.Repositories;
using Astrum.SharedLib.Persistence.Repositories;

namespace Astrum.Identity.Persistence.Repositories.EntityRepositories
{
    public class GitLabUsersRepository : EFRepository<GitlabUser, long, IdentityDbContext>,
    IGitLabUsersRepository
    {
        public GitLabUsersRepository(IdentityDbContext context) : base(context)
        {
        }
    }
}
