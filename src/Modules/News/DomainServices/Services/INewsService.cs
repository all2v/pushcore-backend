using Astrum.News.Application.ViewModels.Requests;
using Astrum.News.ViewModels;
using Astrum.SharedLib.Common.Results;

namespace Astrum.News.Services;

public interface INewsService
{
    Task<Result<NewsForm>> GetNews(CancellationToken cancellationToken = default);
    Task<Result<PostForm>> GetPostById(Guid id, CancellationToken cancellationToken = default);
    Task<Result<List<PostForm>>> GetPostsByUser(Guid userId, CancellationToken cancellationToken = default);
    Task<Result<List<PostForm>>> GetPostsByUser(Guid userId, int? startIndex, int? count);
    Task<Result<PostPaginationView>> GetPostsByUserPagination(Guid userId, int startIndex, int count);
    Task<Result<PostForm>> UpdatePost(Guid id, PostForm form, CancellationToken cancellationToken = default);
    Task<Result<PostForm>> CreatePost(PostRequest form, CancellationToken cancellationToken = default);
    Task<Result<PostForm>> DeletePost(Guid id, CancellationToken cancellationToken = default);
}