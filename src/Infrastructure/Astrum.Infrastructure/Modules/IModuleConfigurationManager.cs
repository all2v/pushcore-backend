using Astrum.Infrastructure.Models;

namespace Astrum.Infrastructure.Modules;

public interface IModuleConfigurationManager
{
    IEnumerable<ModuleInfo> GetModules();
}