using Astrum.Account.Repositories;
using Astrum.Account.Services;
using Astrum.Account.Specifications.RegistrationApplication;
using Astrum.SharedLib.Common.CQS.Implementations;
using Astrum.SharedLib.Common.Results;
using AutoMapper;

namespace Astrum.Account.Features.Registration.Queries.GetAllRegistrationApplications;

public class GetRegistrationApplicationsListQueryHandler : QueryResultHandler<GetRegistrationApplicationsListQuery, 
    List<RegistrationApplicationResponse>>
{
    private readonly IRegistrationApplicationRepository _applicationRepository;
    private readonly IMapper _mapper;

    public GetRegistrationApplicationsListQueryHandler(IRegistrationApplicationRepository applicationRepository, IMapper mapper)
    {
        _applicationRepository = applicationRepository;
        _mapper = mapper;
    }

    public override async Task<Result<List<RegistrationApplicationResponse>>> Handle(
        GetRegistrationApplicationsListQuery query, CancellationToken cancellationToken = default)
    {
        var applicationsSpec = new GetRegistrationApplicationsSpec();
        var applications = await _applicationRepository.ListAsync(applicationsSpec, cancellationToken);
        var response = _mapper.Map<List<RegistrationApplicationResponse>>(applications);
        return Result.Success(response);
    }
}