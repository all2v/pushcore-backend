using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.SharedLib.Application.Contracts.Persistence.Repositories;
using Astrum.SharedLib.Domain.Interfaces;
using Astrum.News.Aggregates;

namespace Astrum.News.Repositories
{
    public interface IWidgetRepository : IEntityRepository<Widget, Guid>
    {
    }
}
