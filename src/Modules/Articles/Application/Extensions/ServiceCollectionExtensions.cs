using System.Reflection;
using Astrum.Articles.Mapping;
using Astrum.Articles.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Astrum.Articles.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddAutoMapper(cfg =>
            {
                cfg.AddMaps(typeof(ArticleProfile));
            });

            services.AddScoped<IArticleService, ArticleService>();
            services.AddScoped<ITagService, TagService>();
            services.AddScoped<ICategoryService, CategoryService>();
        }
    }
}