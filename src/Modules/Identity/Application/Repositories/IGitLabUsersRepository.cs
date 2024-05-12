using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.Identity.Domain.Entities;
using Astrum.Identity.Models;
using Astrum.SharedLib.Application.Contracts.Persistence.Repositories;

namespace Astrum.Identity.Application.Repositories
{
    public interface IGitLabUsersRepository : IEntityRepository<GitlabUser, long>
    {
    }
}
