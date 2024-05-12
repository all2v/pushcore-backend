using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Astrum.Api.Extensions
{
    public static class HttpContextExtensions
    {
        public static string GetCorrelationId(this HttpContext httpContext)
        {
            httpContext.Request.Headers.TryGetValue("Cko-Correlation-Id", out var correlationId);
            return correlationId.FirstOrDefault() ?? httpContext.TraceIdentifier;
        }
    }
}
