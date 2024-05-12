using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Astrum.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;

namespace Astrum.Api.Extensions;

public static class MvcBuilderExtensions
{
    public static IMvcBuilder LoadModules(this IMvcBuilder mvcBuilder, IEnumerable<ModuleInfo> modules)
    {
        foreach (var module in modules.Where(x => !x.IsBundledWithHost))
            AddApplicationPart(mvcBuilder, module.Assembly);

        return mvcBuilder;
    }

    private static void AddApplicationPart(IMvcBuilder mvcBuilder, Assembly assembly)
    {
        var partFactory = ApplicationPartFactory.GetApplicationPartFactory(assembly);
        foreach (var part in partFactory.GetApplicationParts(assembly))
            mvcBuilder.PartManager.ApplicationParts.Add(part);

        var relatedAssemblies = RelatedAssemblyAttribute.GetRelatedAssemblies(assembly, false);
        foreach (var relatedAssembly in relatedAssemblies)
        {
            partFactory = ApplicationPartFactory.GetApplicationPartFactory(relatedAssembly);
            foreach (var part in partFactory.GetApplicationParts(relatedAssembly))
                mvcBuilder.PartManager.ApplicationParts.Add(part);
        }
    }
}