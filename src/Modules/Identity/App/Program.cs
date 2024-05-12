using Astrum.Identity;
using Astrum.Infrastructure.Services.DbInitializer;

//Activity.DefaultIdFormat = ActivityIdFormat.W3C;
//Log.Logger = new LoggerConfiguration()
//    .MinimumLevel.Debug()
//    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
//    .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
//    .MinimumLevel.Override("System", LogEventLevel.Warning)
//    .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
//    .Enrich.FromLogContext()
//    .WriteTo.Console(
//        outputTemplate:
//        "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}",
//        theme: AnsiConsoleTheme.Code)
//    .CreateLogger();

//Log.Information("Starting host...");

try
{
    var builder = WebApplication.CreateBuilder(args);

    //builder.Host.UseSerilog((ctx, lc) => lc
    //    .WriteTo.Console(
    //        outputTemplate:
    //        "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}")
    //    .Enrich.FromLogContext()
    //    .ReadFrom.Configuration(ctx.Configuration));

    var app = builder
        .ConfigureServices()
        .ConfigurePipeline();
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var logger = services.GetRequiredService<ILogger<Program>>();
        try
        {
            logger.LogInformation("Running database migration/seed");

            var dbInitializers = services.GetServices<IDbContextInitializer>();
            foreach (var dbInitializer in dbInitializers)
            {
                await dbInitializer.Migrate();
                await dbInitializer.Seed();
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while running database migration.");
        }
    }

    app.Run();
}
catch (Exception ex)
{
}
finally
{
}