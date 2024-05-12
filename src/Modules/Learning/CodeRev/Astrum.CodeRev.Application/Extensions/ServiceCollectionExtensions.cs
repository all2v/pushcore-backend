using Astrum.CodeRev.Application.CompilerService.Services;
using Astrum.CodeRev.Application.TrackerService.Mappings;
using Astrum.CodeRev.Application.TrackerService.Services;
using Astrum.CodeRev.Application.UserService.Mappings;
using Astrum.CodeRev.Application.UserService.Services;
using Astrum.CodeRev.Application.UserService.Services.Interviews;
using Astrum.CodeRev.Application.UserService.Services.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Astrum.CodeRev.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ICompilerResolver, CompilerResolver>();
        services.AddTransient<CSharpCompilerService>();
        services.AddTransient<JsCompilerService>();
        services.AddScoped<IAssemblyTestingService, CSharpAssemblyTestingService>();
        services.AddScoped<ITaskTestsProviderService, TaskTestsProviderService>();
        services.AddAutoMapper(cfg =>
        {
            cfg.AddMaps(typeof(TrackerServiceProfile));
        });
        services.AddScoped<ITrackerService, TrackerService.Services.TrackerService>();
        services.AddScoped<ITaskRecordDeserializer, TaskRecordDeserializer>();
        services.AddScoped<ITaskRecordSerializer, TaskRecordSerializer>();
        services.AddAutoMapper(cfg =>
        {
            cfg.AddMaps(typeof(UserServiceMapping));
        });
        services.AddScoped<IUserService, UserService.Services.UserService>();
        services.AddScoped<ICardService, CardService>();
        services.AddScoped<IInterviewLanguageService, InterviewLanguageService>();
        services.AddScoped<IInterviewService, InterviewService>();
        services.AddScoped<IStatusCheckerService, StatusCheckerService>();
        services.AddScoped<ITaskService, TaskService>();
        services.AddScoped<IDraftService, DraftService>();
        services.AddScoped<IInvitationService, InvitationService>();
        services.AddScoped<IMeetsService, MeetsService>();
    }
}