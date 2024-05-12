using Astrum.SharedLib.Common.CQS.Implementations;

namespace Astrum.Account.Features.Registration.Queries.GetRegistrationApplication;

public class GetRegistrationApplicationQuery : QueryResult<RegistrationApplicationResponse>
{
    public GetRegistrationApplicationQuery(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}