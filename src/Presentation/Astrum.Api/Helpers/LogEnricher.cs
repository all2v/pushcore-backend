//using System.Linq;
//using Microsoft.AspNetCore.Http;

//namespace Astrum.Api.Helpers
//{
//    public static class LogEnricher
//    {
//        /// <summary>
//        ///     Enriches the HTTP request log with additional data via the Diagnostic Context
//        /// </summary>
//        /// <param name="diagnosticContext">The Serilog diagnostic context</param>
//        /// <param name="httpContext">The current HTTP Context</param>
//        public static void EnrichFromRequest(IDiagnosticContext diagnosticContext, HttpContext httpContext)
//        {
//            diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
//            diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
//            diagnosticContext.Set("ClientIP", httpContext.Connection.RemoteIpAddress?.ToString());
//            diagnosticContext.Set("UserAgent", httpContext.Request.Headers["User-Agent"].FirstOrDefault());
//            // diagnosticContext.Set("Resource", httpContext.GetMetricsCurrentResourceName()); // for azure
//        }
//    }
//}
