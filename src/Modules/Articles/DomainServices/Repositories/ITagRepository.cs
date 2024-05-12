using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.Articles.Aggregates;
using Astrum.SharedLib.Application.Contracts.Persistence.Repositories;

namespace Astrum.Articles.Repositories
{
    public interface ITagRepository : IEntityRepository<Tag, Guid>
    {
    }
}
