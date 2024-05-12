using Astrum.Infrastructure.Services.DbInitializer;
using Astrum.ITDictionary.Repositories;
using Astrum.SharedLib.Persistence.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Astrum.ITDictionary.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCustomDbContext<ITDictionaryDbContext>(
            configuration.GetConnectionString(DbContextExtensions.BaseConnectionName));
        services.AddScoped<IDbContextInitializer, DbContextInitializer>();
        
        services.AddScoped<ITermRepository, TermRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IPracticeRepository, PracticeRepository>();
        services.AddScoped<ITestQuestionRepository, TestQuestionsRepository>();
        services.AddScoped<IAnswerOptionRepository, AnswerOptionRepository>();
        services.AddScoped<IUserTermRepository, UserTermRepository>();
    }
}