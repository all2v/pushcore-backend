using System.Text.Json;
using Astrum.Identity.Contracts;
using Astrum.Identity.Extensions;
using Astrum.Identity.Services;
using Astrum.Infrastructure.Shared.Extensions;

namespace Astrum.Identity;

static internal class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddRazorPages();
        builder.Services.AddIdentityInfrastructureServices(builder.Configuration);
        builder.Services.AddIdentityPersistence(builder.Configuration);

        builder.Services.AddApplicationServices();
        builder.Services.AddBackofficeServices();

        builder.Services.AddScoped<IUserAuthenticationService, UserAuthenticationService>();

        
        builder.Services
            .AddControllers(options =>
            {
                options.UseRoutePrefix("api");
            })
            .AddJsonOptions(x => x.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase)
            .AddNewtonsoftJson();

        builder.Services.AddCors(options =>
        {
            // this defines a CORS policy called "default"
            options.AddPolicy("Policy", policy =>
            {
                // policy.WithOrigins("https://localhost:5003")
                policy.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });
        
        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
            app.UseDeveloperExceptionPage();

        app.UseCors("Policy");
        app.UseStaticFiles();
        app.UseRouting();
        app.UseIdentityServer();

        app.UseAuthorization();
        app.MapRazorPages().RequireAuthorization();
        
        app.MapControllers();
        return app;
    }
}