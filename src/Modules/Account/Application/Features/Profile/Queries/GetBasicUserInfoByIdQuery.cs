using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.SharedLib.Common.CQS.Implementations;

namespace Astrum.Account.Application.Features.Profile.Queries
{
    public class GetBasicUserInfoByIdQuery : QueryResult<BasicUserInfoResponse>
    {
        public GetBasicUserInfoByIdQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
