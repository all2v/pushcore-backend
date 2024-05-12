using Astrum.Api.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Astrum.Api.Extensions
{
    public static class GitlabAuthExtensions
    {
        public static IServiceCollection AddGitLabAuthentication(this IServiceCollection services, IConfigurationSection section)
        {
            var gitlabSettings = new GitlabSettings();
            section.Bind(gitlabSettings);
            services.AddAuthentication(options => {
                //options.DefaultScheme = "Application";
                //options.DefaultSignInScheme = "External";
                //options.DefaultSignInScheme = "External";
            })
              //.AddCookie("Application")
              .AddCookie("External")
              .AddGitLab(options =>
              {
                  options.ClientId = gitlabSettings.ClientId;
                  options.ClientSecret = gitlabSettings.ClientSecret;
                  options.AuthorizationEndpoint = $"{gitlabSettings.HostUrl}/oauth/authorize";
                  options.TokenEndpoint = $"{gitlabSettings.HostUrl}/oauth/token";
                  options.UserInformationEndpoint = $"{gitlabSettings.HostUrl}/api/v4/user";
                  options.CallbackPath = "/signin-gitlab";
                  options.CorrelationCookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Lax;
              });

            return services;

        }
    }
}
