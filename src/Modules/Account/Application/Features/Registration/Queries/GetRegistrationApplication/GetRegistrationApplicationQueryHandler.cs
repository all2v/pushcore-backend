using Astrum.Account.Services;
using Astrum.SharedLib.Common.Results;

namespace Astrum.Account.Features.Registration.Queries.GetRegistrationApplication;

public class GetRegistrationApplicationQueryHandler
    : BaseQueryRegistrationApplicationHandler<GetRegistrationApplicationQuery, RegistrationApplicationResponse>
{
    public GetRegistrationApplicationQueryHandler(IRegistrationApplicationService registrationApplicationService) :
        base(registrationApplicationService)
    {
    }

    public override async Task<Result<RegistrationApplicationResponse>> Handle(GetRegistrationApplicationQuery query,
        CancellationToken cancellationToken = default)
    {
        var application = await RegistrationApplicationService.GetAsync(query.Id);
        return Result.Success(application);
    }
}