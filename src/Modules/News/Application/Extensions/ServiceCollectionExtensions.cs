using Astrum.News.Mappings;
using Astrum.News.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Astrum.News.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        var currentAssembly = typeof(NewsProfile).Assembly;
        services.AddMediatR(currentAssembly);
        services.AddScoped<INewsService, NewsService>();
        services.AddScoped<IBannersService, BannersService>();
        services.AddScoped<IWidgetService, WidgetService>();
        services.AddScoped<ILikesService, LikesService>();
        services.AddScoped<ICommentsService, CommentsService>();
        services.AddAutoMapper(cfg =>
        {
            cfg.AddMaps(currentAssembly);
        });
    }
}