using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.Infrastructure.Services.DbInitializer;
using Astrum.SharedLib.Persistence.Extensions;
using Astrum.TrackerProject.Domain.Aggregates;
using Astrum.TrackerProject.Domain.Repositories;
using Astrum.TrackerProject.Persistance.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Astrum.TrackerProject.Persistance.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("BaseConnection");
            services.AddCustomDbContext<TrackerProjectDbContext>(connectionString);
            services.AddScoped<IDbContextInitializer, DbContextContextInitializer>();

            services.AddScoped<ITrackerProjectRepository<Article>, TrackerProjectRepository<Article>>();
            services.AddScoped<ITrackerProjectRepository<Domain.Aggregates.Project>,
                TrackerProjectRepository<Domain.Aggregates.Project>>();
            services.AddScoped<ITrackerProjectRepository<Issue>, TrackerProjectRepository<Issue>>();
            services.AddScoped<ITrackerProjectRepository<ExternalUser>, TrackerProjectRepository<ExternalUser>>();
        }
    }
}
