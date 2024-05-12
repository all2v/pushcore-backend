using Astrum.Account.Features.Account.AccountDetails;
using Astrum.SharedLib.Common.CQS.Implementations;
using Microsoft.AspNetCore.Mvc;

namespace Astrum.Account.Features.Account.UserEdit.Queries;

public sealed class UserEditQuery : QueryResult<EditUserResponse>
{
    [FromQuery]
    public Guid Id { get; set; }
}