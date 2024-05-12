using Astrum.News.Services;
using Astrum.News.ViewModels;
using HotChocolate;

namespace Astrum.News.GraphQL;

public class QueryNews
{
    public async Task<NewsForm> GetNews([Service] INewsService newsService,
        CancellationToken cancellationToken)
    {
        return await newsService.GetNews(cancellationToken);
    }

    public async Task<PostForm> GetPostById([Service] INewsService newsService, Guid id)
    {
        return await newsService.GetPostById(id);
    }
}