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
using Microsoft.AspNetCore.Http.HttpResults;

namespace Astrum.Account.Features.Profile.Queries;

public class GetEditInfoByUsernameQueryHandler : QueryResultHandler<GetEditInfoByUsernameQuery, EditUserProfileResponse>
{
    private readonly IApplicationUserRepository _userRepository;
    private readonly IUserProfileRepository _profileRepository;
    private readonly IFileStorage _fileStorage;
    private readonly IMapper _mapper;

    public GetEditInfoByUsernameQueryHandler(IApplicationUserRepository userRepository, IUserProfileRepository profileRepository,
        IFileStorage fileStorage, IMapper mapper)
    {
        _userRepository = userRepository;
        _profileRepository = profileRepository;
        _fileStorage = fileStorage;
        _mapper = mapper;
    }

    public override async Task<Result<EditUserProfileResponse>> Handle(GetEditInfoByUsernameQuery query,
        CancellationToken cancellationToken = default)
    {
        var userSpec = new GetUserByUsernameSpec(query.Username);
        var user = await _userRepository.FirstOrDefaultAsync(userSpec, cancellationToken);

        if (user == null)
            return Result.NotFound($"Пользователь не найден.");

        var profileSpec = new GetUserProfileByUserIdSpec(user.Id);
        var profile = await _profileRepository.FirstOrDefaultAsync(profileSpec, cancellationToken);

        if (profile == null)
            return Result.NotFound($"Профиль пользователя не найден.");

        var response = _mapper.Map<EditUserProfileResponse>(profile);
        if (profile.AvatarImageId.HasValue)
            response.AvatarUrl = await _fileStorage.GetFileUrl(profile.AvatarImageId.Value);
        response.Username = user.UserName;
        response.Contacts = new() { Email = user.Email, PhoneNumber = user.PhoneNumber };

        return Result.Success(response);
    }
}