using Astrum.Recruting.Application.Service;
using Astrum.Recruting.Application.Service.HostedServices;
using Astrum.Recruting.Domain.Aggregates.Candidate;
using Microsoft.Extensions.DependencyInjection;

namespace Astrum.Recruting.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IVacancyService, VacancyService>();
        services.AddScoped<IKanbanService, KanbanService>();
        services.AddScoped<IPracticeVacancyService, PracticeVacancyService>();
        services.AddScoped<IHrmUserService, HrmUserService>();
        services.AddScoped<ICandidateService, CandidateService>();
        services.AddScoped(typeof(PracticeLoaderService));
        services.AddScoped(typeof(PracticeDataCollectorService));
        services.AddHostedService<PracticeLoaderHostedService>();
    }
}