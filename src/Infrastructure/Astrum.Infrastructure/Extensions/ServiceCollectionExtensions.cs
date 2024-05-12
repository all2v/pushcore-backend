using System.Diagnostics;
using System.Reflection;
using System.Runtime.Loader;
using Astrum.Infrastructure.Mappings;
using Astrum.Infrastructure.Models;
using Astrum.Infrastructure.Modules;
using Astrum.Infrastructure.Services;
using Astrum.Infrastructure.Services.Shared;
using Astrum.SharedLib.Application.Contracts.Infrastructure;
using Astrum.SharedLib.Application.Contracts.Infrastructure.Shared;
using Astrum.SharedLib.Application.Contracts.Infrastructure.Storage;
using AutoMapper.Extensions.ExpressionMapping;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Astrum.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    private static readonly IModuleConfigurationManager _modulesConfig = new ModuleConfigurationManager();

    public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediator(x =>
        {
        });
        services.AddScoped<IServiceBus, ServiceBusMediator>();
        //services.AddModules();
        services.AddModulesServices(configuration);

        services.AddAutoMapper(cfg =>
        {
            cfg.AddExpressionMapping();
            cfg.AddMaps(typeof(AppProfile));
        });
    }

    public static void AddSharedServices(this IServiceCollection services)
    {
        services.AddScoped<IPasswordGeneratorService, PasswordGeneratorIdentityService>();
        services.AddScoped<IFileStorageService, FileStorageService>();
        services.AddScoped<IEmailService, EmailService>();
    }

    private static void AddModules(this IServiceCollection services)
    {
        var modules = _modulesConfig.GetModules().ToList();
        foreach (var module in modules)
        {
            //ещё в тесте
            if (!module.IsBundledWithHost)
            {
                TryLoadModuleAssembly(module.Id, module);
                if (module.Assembly == null)
                    throw new Exception($"Cannot find main assembly for module {module.Id}");
            }
            //else
            // try
            //{
            // var subMods = new List<string>
            // {
            // "Application",
            // "Backoffice",
            // "Domain",
            // "DomainServices",
            // "Infrastructure",
            // "Persistence",
            // "Startup"
            // };
            // if (module.Id.Contains("Identity"))
            // subMods = new List<string>
            // {
            // //"App",
            // //"Application",
            // //"Backoffice",
            // //"Domain",
            // //"Infrastructure",
            // //"Persistence",
            // "Startup"
            // };
            // foreach (var subMod in subMods)
            // {
            // var assemblyName = new AssemblyName(module.Id + "." + subMod);
            // var assembly = Assembly.Load(assemblyName);
            // module.Assembly = assembly;
            // GlobalConfiguration.Modules.Add(module);
            // }
            //}
            //catch (Exception e)
            //{
            // Console.WriteLine(e);
            // throw;
            //}

            //module.Assembly = Assembly.Load(new AssemblyName(module.Id));
        }
    }

    private static void AddModulesServices(this IServiceCollection services,
    IConfiguration configurationManager)
    {
        // var subMods = new List<string>
        // {
        //     "Application",
        //     "Backoffice",
        //     "Domain",
        //     "DomainServices",
        //     "Infrastructure",
        //     "Persistence",
        //     "Startup"
        // };
        // //var modules = GlobalConfiguration.Modules;
        // var modules = _modulesConfig.GetModules().SelectMany(m => subMods.Select(sm => m.Id + "." + sm)).ToList();
        // modules.Remove("Astrum.Identity.Backoffice");
        // modules.Remove("Astrum.Identity.DomainServices");
        // foreach (var module in modules)
        // {
        //     var assembly = Assembly.Load(new AssemblyName(module));
        //     var types = assembly.GetTypes();
        //     var moduleInitializerType = types
        //     .FirstOrDefault(t => typeof(IModuleInitializer).IsAssignableFrom(t));
        //     if (moduleInitializerType != null && moduleInitializerType != typeof(IModuleInitializer))
        //     {
        //         var moduleInitializer = (IModuleInitializer)Activator.CreateInstance(moduleInitializerType);
        //         services.AddSingleton(typeof(IModuleInitializer), moduleInitializer);
        //         moduleInitializer.ConfigureServices(services, configurationManager);
        
        //     }
        // }
        var assemblyLocation = Assembly.GetExecutingAssembly().Location;
        var assemblyFolder = Path.GetDirectoryName(assemblyLocation);
        var files = Directory.GetFiles(assemblyFolder, "*.Startup.dll");
        var loadedStartupProjects = files.Select(Assembly.LoadFile).ToList();
        var moduleInitializerTypes = loadedStartupProjects
            .Select(x => x.GetTypes()
                .FirstOrDefault(t => typeof(IModuleInitializer).IsAssignableFrom(t))
            )
            .Where(x => x is not null)
            .ToList();
        foreach (var moduleInitializerType in moduleInitializerTypes)
        {
            var moduleInitializer = (IModuleInitializer)Activator.CreateInstance(moduleInitializerType);
            services.AddSingleton(typeof(IModuleInitializer), moduleInitializer);
            moduleInitializer.ConfigureServices(services, configurationManager);
        }
    }

    /// <summary>
    /// ещё в тесте
    /// </summary>
    /// <param name="moduleFolderPath"></param>
    /// <param name="module"></param>
    /// <exception cref="Exception"></exception>
    private static void TryLoadModuleAssembly(string moduleFolderPath, ModuleInfo module)
    {
        var startupProjectName = StartupProjectName(moduleFolderPath, module, out var binariesFolderPath, out var binariesFolder);

        if (Directory.Exists(binariesFolderPath))
        {
            var files = binariesFolder.GetFileSystemInfos("*.dll", SearchOption.AllDirectories);
            foreach (var file in files) //TODO check it because subdirectories could not load
            {
                Assembly assembly;
                try
                {
                    assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(file.FullName);
                }
                catch (FileLoadException)
                {
                    // Get loaded assembly. This assembly might be loaded
                    assembly = Assembly.Load(new AssemblyName(Path.GetFileNameWithoutExtension(file.Name)));

                    if (assembly == null) throw;

                    var loadedAssemblyVersion = FileVersionInfo.GetVersionInfo(assembly.Location).FileVersion;
                    var tryToLoadAssemblyVersion = FileVersionInfo.GetVersionInfo(file.FullName).FileVersion;

                    // Or log the exception somewhere and don't add the module to list so that it will not be initialized
                    if (tryToLoadAssemblyVersion != loadedAssemblyVersion)
                        throw new Exception(
                        $"Cannot load {file.FullName} {tryToLoadAssemblyVersion} because {assembly.Location} {loadedAssemblyVersion} has been loaded");
                }

                if (Path.GetFileNameWithoutExtension(assembly.ManifestModule.Name) == startupProjectName) // TO
                    module.Assembly = assembly;
            }
        }
    }

    private static string StartupProjectName(string moduleFolderPath, ModuleInfo module, out string binariesFolderPath,
    out DirectoryInfo binariesFolder)
    {
        const string binariesFolderName = "bin";
        const string modulesName = "Modules";
        const string startupProjectPostfix = "Startup";
        var startupProjectName = $"{module.Id}.{startupProjectPostfix}";
        const string top = @"..\..\";
        var moduleNameIndex = moduleFolderPath.LastIndexOf(".", StringComparison.Ordinal) + 1;
        var moduleName = moduleFolderPath[moduleNameIndex..];
        binariesFolderPath = Path.Combine(top, modulesName, moduleName, startupProjectPostfix, binariesFolderName);
        binariesFolder = new DirectoryInfo(binariesFolderPath);
        var currectDirectory = Directory.GetCurrentDirectory();
        return startupProjectName;
    }
}