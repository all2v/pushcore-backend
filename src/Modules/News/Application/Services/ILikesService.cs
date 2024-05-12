using Astrum.News.DomainServices.ViewModels.Requests;
using Astrum.News.ViewModels;

namespace Astrum.News.Services;

public interface ILikesService
{
    Task<SharedLib.Common.Results.Result<LikeForm>> GetLikeById(Guid id, CancellationToken cancellationToken = default);
    Task<int> GetLikesCountByPost(Guid postId);
    Task<Guid?> GetLikeIdByPostAndUser(Guid postId, string userId);
    Task<SharedLib.Common.Results.Result<LikeForm>> CreateLike(LikeRequest form, CancellationToken cancellationToken = default);
    Task<SharedLib.Common.Results.Result<LikeForm>> DeleteLike(Guid id, CancellationToken cancellationToken = default);
    Task<SharedLib.Common.Results.Result<LikeForm>> DeleteLike(LikeDeleteRequest request,
        CancellationToken cancellationToken = default);
    Task SetUserToLike(LikeForm form);
    Task SetUserToLikesList(List<LikeForm> forms, Dictionary<Guid, UserInfo> fetchedUsers = null);
}