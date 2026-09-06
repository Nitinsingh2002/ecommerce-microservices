

using buildingblocks.Common.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace Common.Extensions;

public static class ApplicationBuilderExtensions
{
 public static IApplicationBuilder UseGlobalExceptionalHandler( this IApplicationBuilder app)
    {
        return app.UseMiddleware<GlobalExceptionMiddleware>();
    }
}