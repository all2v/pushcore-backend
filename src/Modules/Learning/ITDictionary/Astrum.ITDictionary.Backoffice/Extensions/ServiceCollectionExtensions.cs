using System.Reflection;
using Astrum.ITDictionary.Controllers;
using Astrum.ITDictionary.Mappings;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Astrum.ITDictionary.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddBackofficeServices(this IServiceCollection services)
    {
        var currentAssembly = typeof(TermsController).Assembly;

        services.AddMediatR(currentAssembly);
    }
}