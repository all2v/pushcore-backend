using Astrum.Infrastructure.Modules;
using Astrum.News.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

//using Astrum.News.Application.Extensions;
//using Astrum.News.Backoffice.Extensions;
//using Astrum.News.Infrastructure.Extensions;
//using Astrum.News.Persistence.Extensions;

namespace Astrum.News;

public class ModuleInitializer : IModuleInitializer
{
    #region IModuleInitializer Members

    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddApplicationServices();
        services.AddPersistenceServices(configuration);
        services.AddInfrastructureServices(configuration);
        services.AddBackofficeServices();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapGraphQL("/graphql/news", "NewsSchema");
        });
    }

    #endregion
}