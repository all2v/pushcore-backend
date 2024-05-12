using Astrum.Identity.Contracts;
using Astrum.Identity.ReadModels;
using Astrum.SharedLib.Common.CQS.Implementations;
using Astrum.SharedLib.Common.Options;
using Astrum.SharedLib.Common.Results;
using AutoMapper;

namespace Astrum.Identity.Features.Queries;

public class GetAllUsersQuery : QueryResult<IEnumerable<UserReadModel>>
{
    public QueryOptions Options { get; set; }
    public bool ApplyRoleFilter { get; set; } = true;
}

public class GetAllUsersQueryHandler : QueryResultHandler<GetAllUsersQuery, IEnumerable<UserReadModel>>
{
    private readonly IApplicationUserService _applicationUserService;
    private readonly IAuthenticatedUserService _authenticatedUserService;
    private readonly IMapper _mapper;

    public GetAllUsersQueryHandler(IApplicationUserService applicationUserService, IMapper mapper,
        IAuthenticatedUserService authenticatedUserService)
    {
        _applicationUserService = applicationUserService;
        _mapper = mapper;
        _authenticatedUserService = authenticatedUserService;
    }

    public override async Task<Result<IEnumerable<UserReadModel>>> Handle(GetAllUsersQuery query,
        CancellationToken cancellationToken = default)
    {
        var getUsersResult = await _applicationUserService.GetAllUsersAsync(query.Options, cancellationToken);

        if (getUsersResult.Failed)
            return Result.Error();
        var data = _mapper.Map<IEnumerable<UserReadModel>>(getUsersResult.Data);

        if (query.ApplyRoleFilter)
        {
            var currentUserHighestRole = _authenticatedUserService.Roles.Max();
            data = data.Where(u => u.Roles.Max() <= currentUserHighestRole);
        }

        return Result.Success(data);
        // result.Successful().WithData(data);
        // result.AddMetadata("RecordCount", data.Count());
    }
}