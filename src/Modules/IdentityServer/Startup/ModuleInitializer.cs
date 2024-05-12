using Api.Application.Extensions;
using Astrum.Example.Extensions;
using Astrum.IdentityServer.Infrastructure.Extensions;
using Astrum.Infrastructure.Modules;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Astrum.Example;

public class ModuleInitializer : IModuleInitializer
{
    #region IModuleInitializer Members

    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        // services.AddApplicationServices();
        // services.AddPersistenceServices(configuration);
        // services.AddInfrastructureServices(configuration);
        // services.AddDomainServices();
        // services.AddBackofficeServices();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
    }

    #endregion
}