

using buildingblocks.Common.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace Common.Extensions;
using Common.Middlewares;

public static class ApplicationBuilderExtensions
{
 public static IApplicationBuilder UseGlobalExceptionalHandler( this IApplicationBuilder app)
    {
        return app.UseMiddleware<GlobalExceptionMiddleware>();
    }

    public static IApplicationBuilder UseCorrelationId(this IApplicationBuilder app)
    {
        return app.UseMiddleware<CorrelationMiddleware>();
    }
    
    public static IApplicationBuilder UseRequestResponseLogging(this IApplicationBuilder app)
    {
        return app.UseMiddleware<RequestLoggingMiddleware>();
    }
}