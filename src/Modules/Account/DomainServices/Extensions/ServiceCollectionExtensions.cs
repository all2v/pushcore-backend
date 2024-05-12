using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Astrum.Account.DomainServices.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddDomainServices(this IServiceCollection services)
    {
        var currentAssembly = Assembly.GetExecutingAssembly();
        services.AddMediatR(currentAssembly);
    }
}