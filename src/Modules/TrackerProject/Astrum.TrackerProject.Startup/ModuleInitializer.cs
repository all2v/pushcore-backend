// using Astrum.Infrastructure.Modules;
// using Astrum.TrackerProject.Application.Extensions;
// using Astrum.TrackerProject.Persistance.Extensions;
// using Microsoft.AspNetCore.Builder;
// using Microsoft.AspNetCore.Hosting;
// using Microsoft.Extensions.Configuration;
// using Microsoft.Extensions.DependencyInjection;
//
// namespace Astrum.TrackerProject.Startup;
//
// public class ModuleInitializer : IModuleInitializer
// {
//     #region IModuleInitializer Members
//
//     public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
//     {
//         services.AddApplicationServices();
//         services.AddPersistenceServices(configuration);
//     }
//
//     public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
//     {
//     }
//
//     #endregion
// }