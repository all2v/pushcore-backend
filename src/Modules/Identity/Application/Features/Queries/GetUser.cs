using Astrum.Identity.Contracts;
using Astrum.Identity.ReadModels;
using Astrum.SharedLib.Common.CQS.Implementations;
using Astrum.SharedLib.Common.Results;
using AutoMapper;

namespace Astrum.Identity.Features.Queries;

public sealed class GetUser : QueryResult<UserReadModel>
{
    public Guid Id { get; set; }
}

public sealed class GetUserQueryHandler : QueryResultHandler<GetUser, UserReadModel>
{
    private readonly IApplicationUserService _applicationUserService;
    private readonly IMapper _mapper;

    public GetUserQueryHandler(IApplicationUserService applicationUserService, IMapper mapper)
    {
        _applicationUserService = applicationUserService;
        _mapper = mapper;
    }

    public override async Task<Result<UserReadModel>> Handle(GetUser query,
        CancellationToken cancellationToken = default)
    {
        var userResult = await _applicationUserService.GetUserAsync(query.Id, cancellationToken);

        if (!userResult.IsSuccess)
            return Result.Error();

        return Result.Success(_mapper.Map<UserReadModel>(userResult.Data));
    }
}