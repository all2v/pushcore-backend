using System.Reflection;
using Astrum.Projects.Mappings;
using Astrum.Projects.Services;
using AutoMapper.EquivalencyExpression;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Astrum.Projects.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddAutoMapper(cfg =>
            {
                cfg.AddMaps(typeof(ProjectProfile));
                cfg.AddCollectionMappers();
            });

            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<ICustomFieldService, CustomFieldService>();
        }
    }
}
