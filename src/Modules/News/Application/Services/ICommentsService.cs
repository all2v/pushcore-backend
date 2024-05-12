using Astrum.News.DomainServices.ViewModels.Requests;
using Astrum.News.ViewModels;

namespace Astrum.News.Services;

public interface ICommentsService
{
    Task<SharedLib.Common.Results.Result<CommentForm>> GetCommentById(Guid id, CancellationToken cancellationToken = default);
    Task<SharedLib.Common.Results.Result<CommentForm>> UpdateComment(Guid id, CommentForm form, CancellationToken cancellationToken = default);
    Task<SharedLib.Common.Results.Result<CommentForm>> CreateComment(CommentRequest form, CancellationToken cancellationToken = default);
    Task<SharedLib.Common.Results.Result<CommentForm>> DeleteComment(Guid id, CancellationToken cancellationToken = default);
    Task SetUserToComment(CommentForm form);
    Task SetUserToCommentList(List<CommentForm> forms,
        Dictionary<Guid, UserInfo> fetchedUsers = null);
    Task<int> GetCommentsCountByPost(Guid postId);
}