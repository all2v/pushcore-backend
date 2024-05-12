using Astrum.Identity.Extensions;
using Astrum.Infrastructure.Modules;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Astrum.Identity;

public class ModuleInitializer : IModuleInitializer
{
    #region IModuleInitializer Members

    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddApplicationServices();
        services.AddIdentityPersistence(configuration);
        //services.AddCustomizedIdentity(configuration);
        //services.AddIdentityServices();
        services.AddIdentityApplicationServices();
        services.AddIdentityInfrastructureServices(configuration);
        services.AddBackofficeServices();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
    }

    #endregion
}