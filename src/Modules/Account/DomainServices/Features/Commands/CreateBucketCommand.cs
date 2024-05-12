using Astrum.SharedLib.Common.CQS.Implementations;
using Astrum.SharedLib.Common.Results;

namespace Astrum.Account.DomainServices.Features.Commands;

public sealed class CreateBucketCommand : Command<Result<Guid>>
{
    public CreateBucketCommand(Guid userId)
    {
        UserId = userId;
    }
    
    public Guid UserId { get; set; }
}