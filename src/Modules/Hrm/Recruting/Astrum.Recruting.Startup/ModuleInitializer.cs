using Microsoft.Extensions.DependencyInjection;
using Astrum.Infrastructure.Modules;
using Astrum.Recruting.Application.Extensions;
using Astrum.Recruting.Backoffice;
using Astrum.Recruting.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Astrum.Recruting.Startup;

public class ModuleInitializer : IModuleInitializer
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        //services.AddCustomizedIdentity(configuration);
        services.AddApplicationServices();
        services.AddBackofficeServices();
        services.AddPersistenceServices(configuration);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
    }
}