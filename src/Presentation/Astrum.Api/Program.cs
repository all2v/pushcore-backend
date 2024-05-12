using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Astrum.Infrastructure.Services.DbInitializer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Fluent;
using NLog.Web;

namespace Astrum.Api;

public class Program
{
    /// <summary>
    /// </summary>
    /// <param name="args"></param>
    public static async Task Main(string[] args)
    {
        var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
        var host = CreateHostBuilder(args).Build();

        // this seeding is only for the template to bootstrap the DB and users.
        // in production you will likely want a different approach.
        await DatabaseInitialising(args, host);

        await host.RunAsync();
    }

    private static async Task DatabaseInitialising(string[] args, IHost host)
    {
        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILogger<Program>>();
            try
            {
                var dbInitializers = services.GetServices<IDbContextInitializer>();
                foreach (var dbInitializer in dbInitializers)
                {
                    if (dbInitializer is Astrum.Example.DbContextContextInitializer)
                        continue;
                    // if (args.Contains("/init") || args.Contains("/migrate"))
                    await dbInitializer.Migrate();
                    // if (args.Contains("/init") || args.Contains("/seed"))
                    await dbInitializer.Seed();
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while running database migration.");
            }
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStaticWebAssets();
                webBuilder.ConfigureLogging(logBuilder =>
                {
                    logBuilder.ClearProviders();
                    logBuilder.AddConsole();

                    logBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Debug);

                });
                webBuilder.UseKestrel(options =>
                {
                    options.Limits.MaxRequestHeadersTotalSize = 1048576;
                });
                webBuilder.UseStartup<Startup>();
                // webBuilder.UseKestrel(options =>
                //     {
                //         options.Limits.MaxRequestHeadersTotalSize = 1048576;
                //     });
            }).UseNLog();
    }
}