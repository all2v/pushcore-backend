using Astrum.Infrastructure.Extensions;
using Astrum.Infrastructure.Modules;
using Astrum.SampleData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Astrum.SampleData.Startup;

public class ModuleInitializer : IModuleInitializer
{
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
    }

    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddApplicationServices();
        services.AddPersistenceServices(configuration);
        //services.AddBackofficeServices();
    }
}