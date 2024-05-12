using Astrum.Account.Repositories;
using Astrum.Account.Services;
using Astrum.Account.Specifications.UserProfile;
using Astrum.Identity.Contracts;
using Astrum.Identity.Repositories;
using Astrum.Identity.Specifications;
using Astrum.SharedLib.Common.CQS.Implementations;
using Astrum.SharedLib.Common.Results;
using Astrum.Storage.Services;
using AutoMapper;

namespace Astrum.Account.Features.Profile.Queries;

public class GetUsersProfilesQueryHandler : QueryResultHandler<GetUsersProfilesQuery, List<UserProfileSummary>>
{
    private readonly IUserProfileRepository _profileRepository;
    private readonly IFileStorage _fileStorage;
    private readonly IMapper _mapper;
    private readonly IApplicationUserService _applicationUserService;

    public GetUsersProfilesQueryHandler(IUserProfileRepository profileRepository,
        IFileStorage fileStorage,
        IMapper mapper,
        IApplicationUserService applicationUserService)
    {
        _profileRepository = profileRepository;
        _fileStorage = fileStorage;
        _mapper = mapper;
        _applicationUserService = applicationUserService;
    }

    public override async Task<Result<List<UserProfileSummary>>> Handle(GetUsersProfilesQuery query,
        CancellationToken cancellationToken = default)
    {
        var profilesSpec = new GetUsersProfilesSpec(query.Name, query.PositionIds);
        var profiles = await _profileRepository.ListAsync(profilesSpec, cancellationToken);
        
        var summaries = _mapper.Map<List<UserProfileSummary>>(profiles);
        foreach(var summary in summaries)
        {
            var user = await _applicationUserService.GetUserAsync(summary.UserId);
            if (user?.Data == null || !user.IsSuccess)
                continue;
        
            summary.Username = user.Data.UserName;
            summary.Role = user.Data.Roles.FirstOrDefault()?.Role.Name;
            
            if (summary.AvatarImageId.HasValue)
                summary.AvatarUrl = await _fileStorage.GetFileUrl(summary.AvatarImageId.Value);
        }
        summaries.RemoveAll(summary => summary.Username == null);
        
        return Result.Success(summaries);
    }
}