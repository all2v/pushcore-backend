using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.SharedLib.Common.CQS.Implementations;
using YouTrackSharp.Generated;

namespace Astrum.Account.Application.Features.Profile.Queries
{
    public class GetFullNameQuery : QueryResult<FullNameResponce>
    {
        public GetFullNameQuery(string username)
        {
            Username = username;
        }

        public string Username { get; set; }
    }
}
