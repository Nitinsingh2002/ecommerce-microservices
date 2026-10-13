

using System.Threading.RateLimiting;

namespace Authentication.Api.DependencyInjection;

public static class ApiServiceRegistration
{
   public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            options.AddPolicy("LoginPolicy", HttpContext =>
              RateLimitPartition.GetFixedWindowLimiter(
                  partitionKey: HttpContext.Connection.RemoteIpAddress.ToString() ?? "unknown",
                  factory: _ => new FixedWindowRateLimiterOptions{
                      PermitLimit = 5,
                        Window = TimeSpan.FromMinutes(1),
                        QueueLimit = 0,
                        AutoReplenishment = true

                  }));
        });

        return services;
    }
}