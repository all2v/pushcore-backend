using System.Reflection;
using Astrum.Infrastructure.Models;

namespace Astrum.Infrastructure;

public static class GlobalConfiguration
{
    public static IList<ModuleInfo> Modules { get; set; } = new List<ModuleInfo>();

    public static string? WebRootPath { get; set; }

    public static string? ContentRootPath { get; set; }

    public static IEnumerable<Assembly> ModulesAssembly()
    {
        if (Modules != null && Modules.Any())
            foreach (var module in Modules)
                if (module.Assembly != null)
                    yield return module.Assembly;
    }
}