using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.Account.Services;
using Astrum.SharedLib.Domain.Interfaces;
using Astrum.TrackerProject.Application.Mappings;
using Astrum.TrackerProject.Application.Services;
using Astrum.TrackerProject.Persistance;
using AutoMapper;
using AutoMapper.EquivalencyExpression;
using Microsoft.Extensions.DependencyInjection;

namespace Astrum.TrackerProject.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            var currentAssembly = typeof(TrackerProjectProfile).Assembly;

            services.AddAutoMapper((serviceProvider, automapper) =>
            {
                automapper.AddMaps(currentAssembly);
                //automapper.AddCollectionMappers();
                automapper.UseEntityFrameworkCoreModel<TrackerProjectDbContext>(serviceProvider);
            }, typeof(IEntity).Assembly,
            typeof(TrackerProjectDbContext).Assembly);

            //services.AddSingleton(provider => new MapperConfiguration(cfg =>
            //{
            //    cfg.AddProfile(new TrackerProjectProfile(provider.GetService<IUserProfileService>()));
            //}).CreateMapper());

            services.AddScoped<IArticleService, ArticleService>();
            services.AddScoped<IIssueService, IssueService>();
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<ISynchronisationService, SynchronisationService>();
            services.AddScoped<IExternalUserService, ExternalUserService>();
        }
    }
}
