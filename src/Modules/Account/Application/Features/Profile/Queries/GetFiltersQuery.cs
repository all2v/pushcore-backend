using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.Account.Application.ViewModels;
using Astrum.SharedLib.Application.Models.Filters;
using Astrum.SharedLib.Common.CQS.Implementations;

namespace Astrum.Account.Application.Features.Profile.Queries
{
    public class GetFiltersQuery : QueryResult<FilterResponse>
    {
    }
}
