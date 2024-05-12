using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Astrum.Articles.Controllers;
using Astrum.Articles.Mappings;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Astrum.Articles.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddBackofficeServices(this IServiceCollection services)
        {
            var currentAssembly = typeof(ArticlesController).Assembly;

            services.AddMediatR(currentAssembly);
            services.AddAutoMapper(cfg =>
            {
                cfg.AddMaps(typeof(AuthorProfie));
            });
            services.AddMediatR(Assembly.GetExecutingAssembly());
            //services.AddGraphQLServer("MarketSchema")
            //    .AddFiltering()
            //    .AddSorting()
            //    .AddQueryType<QueryMarket>()
            //    .AddSubscriptionType<SubscriptionMarket>();
        }
    }
}
