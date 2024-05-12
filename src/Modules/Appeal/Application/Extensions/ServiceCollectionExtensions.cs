using Astrum.Appeal.Application.Services;
using Astrum.Appeal.Mappings;
using Astrum.Appeal.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Astrum.Appeal.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        var currentAssembly = typeof(AppealProfile).Assembly;
        services.AddMediatR(currentAssembly);
        services.AddScoped<IAppealService, AppealService>();
        services.AddScoped<IAppealCategoryService, AppealCategoryService>();
        services.AddAutoMapper(cfg =>
        {
            cfg.AddMaps(currentAssembly);
        });

        // services.AddTransient<ITypeConverter<Domain.Aggregates.Appeal, AppealForm>, AppealConverter>();
        // services.AddTransient<ITypeConverter<AppealForm, AppealFormResponse>, AppealFormAppealFormResponseConverter>();
        // services.AddTransient<ITypeConverter<AppealForm, Domain.Aggregates.Appeal>, AppealFormAppealConverter>();
        // services.AddTransient<ITypeConverter<Domain.Aggregates.Appeal, AppealForm>, AppealAppealFormConverter>();
        services.AddTransient<AppealConverter>();
        services.AddTransient<AppealFormAppealFormResponseConverter>();
        services.AddTransient<AppealFormAppealConverter>();
        services.AddTransient<AppealAppealFormConverter>();
    }
}