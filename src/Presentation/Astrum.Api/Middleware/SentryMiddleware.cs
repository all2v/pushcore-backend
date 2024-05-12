using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using SharpRaven;
using SharpRaven.Data;

namespace Astrum.Api.Middleware
{
    public static class SentryMiddleware
    {
        public static void UseSentry(this IApplicationBuilder builder)
        {
            builder.Use(async (context, next) =>
            {
                try
                {
                    await next.Invoke();
                }
                catch (Exception e)
                {
                    var client = context.RequestServices.GetService<IRavenClient>();
                    if (client != null)
                    {
                        var sEvent = new SentryEvent(e);
                        sEvent.Tags.Add("UserName", context.User?.Identity?.Name ?? "");
                        sEvent.Tags.Add("Path", context.Request?.Path.ToString() ?? "");
                        var id = await client.CaptureAsync(sEvent);
                        if (id != null && !context.Response.HasStarted)
                        {
                            context.Response.Headers.Add("X-Sentry-Id", id);
                        }
                    }

                    throw;
                }
            });
        }
    }
}
