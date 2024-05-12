using Astrum.Infrastructure.Models;
using Newtonsoft.Json;

namespace Astrum.Infrastructure.Modules;

public class ModuleConfigurationManager : IModuleConfigurationManager
{
    public static readonly string ModulesFilename = "modules.json";

    #region IModuleConfigurationManager Members

    public IEnumerable<ModuleInfo> GetModules()
    {
        var currentPath = GlobalConfiguration.ContentRootPath;

        var modulesPath = Path.Combine(currentPath, ModulesFilename);
        using var reader = new StreamReader(modulesPath);
        var content = reader.ReadToEnd();
        dynamic modulesData = JsonConvert.DeserializeObject(content);
        foreach (var module in modulesData)
            yield return new ModuleInfo
            {
                Id = module.id,
                Version = Version.Parse(module.version.ToString()),
                IsBundledWithHost = module.isBundledWithHost
            };
    }

    #endregion
}