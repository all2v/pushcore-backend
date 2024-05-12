//using Microsoft.AspNetCore.Http;
//using System.Threading.Tasks;
//using Astrum.Api.Extensions;

//namespace Astrum.Api.Middleware;


//public class RequestLogContextMiddleware
//{
//    private readonly RequestDelegate _next;

//    public RequestLogContextMiddleware(RequestDelegate next)
//    {
//        _next = next;
//    }

//    public Task Invoke(HttpContext context)
//    {
//        using (LogContext.PushProperty("CorrelationId", context.GetCorrelationId()))
//            return _next.Invoke(context);
//    }
//}