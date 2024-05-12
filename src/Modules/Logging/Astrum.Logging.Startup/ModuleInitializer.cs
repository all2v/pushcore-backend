using Astrum.Infrastructure.Modules;
using Astrum.Infrastructure.Services.DbInitializer;
using Astrum.Logging.Mappings;
using Astrum.Logging.Repositories;
using Astrum.Logging.Services;
using Astrum.SharedLib.Persistence.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Astrum.Logging.Startup
{
    public class ModuleInitializer : IModuleInitializer
    {
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddCustomDbContext<LogsDbContext>(configuration.GetConnectionString(DbContextExtensions.BaseConnectionName));
            services.AddScoped<IDbContextInitializer, LogsDbContextInitializer>();
            services.AddScoped<ILogHttpRepository, LogHttpRepository>();
            services.AddScoped<ILogHttpService, LogHttpService>();
            services.AddScoped<ILogAdminRepository, LogAdminRepository>();
            services.AddScoped<ILogAdminService, LogAdminService>();

            services.AddAutoMapper(cfg =>
            {
                cfg.AddMaps(typeof(LogsProfile));
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            
        }
    }
}
