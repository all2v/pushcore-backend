using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Astrum.SharedLib.Persistence.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddCustomDbContext<TDbContext>(this IServiceCollection services,
        string connectionString)
        where TDbContext : DbContext
    {
        var moduleAssemblyName = typeof(TDbContext).Assembly.FullName;
        var schemaName = typeof(TDbContext).Name.Replace("DbContext", "");
        var migrationHistoryTableName = $"__{schemaName}_MigrationsHistory";
        var logger = LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
        });
        services.AddDbContext<TDbContext>((_, options) =>
            {
                options.UseNpgsql(connectionString,
                    b => b.MigrationsAssembly(moduleAssemblyName)
                        .MigrationsHistoryTable(migrationHistoryTableName, schemaName)
                );
         
                options.UseLoggerFactory(logger);
                options.EnableSensitiveDataLogging();
            }
        );
    }
}