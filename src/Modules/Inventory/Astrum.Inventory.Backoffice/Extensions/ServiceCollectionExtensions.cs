using System.Reflection;
using Astrum.Inventory.Backoffice.Controllers;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Astrum.Inventory.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddBackofficeServices(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
        }
    }
}
