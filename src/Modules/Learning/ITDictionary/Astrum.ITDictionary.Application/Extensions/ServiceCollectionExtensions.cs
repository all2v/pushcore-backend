using Astrum.ITDictionary.Enums;
using Astrum.ITDictionary.Mappings;
using Astrum.ITDictionary.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Astrum.ITDictionary.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(typeof(CategoryService));
        services.AddAutoMapper(c =>
        {
            c.AddMaps(typeof(ITDictionaryProfile));
        });

        services.AddScoped<ITermService, TermService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IPracticeService, PracticeService>();
        services.AddScoped<ITestQuestionService, TestQuestionService>();
        services.AddScoped<ITermConstructorService, TermConstructorService>();
        services.AddScoped<IStatisticsService, StatisticsService>();

        services.AddSingleton<Random>();
    }
}