//using System;
//using System.IO;
//using Microsoft.AspNetCore.Builder;
//using Microsoft.Extensions.Configuration;

//namespace Astrum.Api.Extensions;

//public static partial class ServiceCollectionExtensions
//{
//    private static IConfigurationRoot _configurationRoot;

//    public static WebApplicationBuilder AddSerilog(this WebApplicationBuilder builder)
//    {
//        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

//        var configuration =  new ConfigurationBuilder()
//            .SetBasePath(Directory.GetCurrentDirectory())
//            .AddJsonFile($"appsettings.{environment}.json")
//            .AddEnvironmentVariables()
//            .Build();

//        Log.Logger = new LoggerConfiguration()
//            .ReadFrom.Configuration(configuration)
//            .WriteTo.SpectreConsole(
//                "",
//                LogEventLevel.Verbose)
//            .CreateLogger();

//        builder.Host.UseSerilog();

//        return builder;
//    }
//}