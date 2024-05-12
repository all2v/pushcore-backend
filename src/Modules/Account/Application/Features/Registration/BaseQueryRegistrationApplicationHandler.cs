using Astrum.Account.Services;
using Astrum.SharedLib.Common.CQS.Implementations;

namespace Astrum.Account.Features.Registration;

public abstract class BaseQueryRegistrationApplicationHandler<TQuery, TResponse> : QueryResultHandler<TQuery, TResponse>
    where TQuery : QueryResult<TResponse> where TResponse : class
{
    protected readonly IRegistrationApplicationService RegistrationApplicationService;

    public BaseQueryRegistrationApplicationHandler(IRegistrationApplicationService registrationApplicationService)
    {
        RegistrationApplicationService = registrationApplicationService;
    }
}