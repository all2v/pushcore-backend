using Astrum.Account.Aggregates;
using Astrum.Account.Application.ViewModels;
using Astrum.Account.Features.Profile;
using Astrum.Account.Features.Profile.Commands;
using Astrum.Identity.Models;
using Astrum.Market.Domain.Enums;
using Astrum.News.ViewModels;
using Astrum.SharedLib.Common.Results;

namespace Astrum.Account.Services;

public interface IUserProfileService
{
    public Task CreateUserProfileAsync(UserProfile profile);
    public Task<Result<UserProfileResponse>> GetUserProfileAsync(Guid userId);
    public Task<Result<UserProfileSummary>> GetUserProfileSummaryAsync(Guid userId);
    public Task<Result<List<UserProfileSummary>>> GetUsersProfilesSummariesAsync(IEnumerable<Guid> usersIds);
    public Task<Result<List<UserProfileSummary>>> GetAllUsersProfilesSummariesAsync(UsersFilter? filter = null);
    public Task<Result<UserProfileSummary>> GetUserProfileSummaryAsync(string username);
    public Task<Result<List<UserProfileSummary>>> GetUsersProfilesSummariesAsync(IEnumerable<string> usernames);

    public Task<Result<UserProfileResponse>> GetUserProfileByUsernameAsync(string username);
    public Task<Result<EditUserProfileResponse>> GetEditInfoByUsernameAsync(string username);
    public Task<Result<EditUserProfileResponse>> EditUserProfileAsync(EditUserProfileCommand command);
    public Task<Result<ApplicationUser>> GetUserByUsername(string username);
    public Task ResetTimelines();
}