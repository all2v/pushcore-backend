using Astrum.CodeRev.Domain.Repositories;
using Astrum.CodeRev.Persistence.MongoSettings;
using Astrum.CodeRev.Persistence.Repositories;
using Astrum.CodeRev.UserService.DomainService.Repositories;
using Astrum.Infrastructure.Services.DbInitializer;
using Astrum.SharedLib.Persistence.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Astrum.CodeRev.Persistence.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<TaskRecordsTrackerDataBaseSettings>(_ => new TaskRecordsTrackerDataBaseSettings
        {
            ConnectionString = "mongodb://localhost:27017",
            DataBaseName = "TrackerDB",
            TaskRecordsCollectionName = "TaskRecords"
        });
        services.AddScoped<ITaskRecordRepository, TaskRecordRepository>();
        
        var connectionString = configuration.GetConnectionString(DbContextExtensions.BaseConnectionName);
        services.AddCustomDbContext<CodeRevDbContext>(connectionString);
        services.AddScoped<IDbContextInitializer, CodeRevDbContextInitializer>();
        services.AddScoped<IInterviewLanguageRepository, InterviewLanguageRepository>();
        services.AddScoped<IInterviewRepository, InterviewRepository>();
        services.AddScoped<IInterviewSolutionRepository, InterviewSolutionRepository>();
        services.AddScoped<IInvitationRepository, InvitationRepository>();
        services.AddScoped<IReviewerDraftRepository, ReviewerDraftRepository>();
        services.AddScoped<ITaskSolutionRepository, TaskSolutionRepository>();
        services.AddScoped<ITestTaskRepository, TestTaskRepository>();
    }
}