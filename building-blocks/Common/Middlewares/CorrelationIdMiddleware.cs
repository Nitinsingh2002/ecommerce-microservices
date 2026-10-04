
using System.Diagnostics;
using Common.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Common.Middlewares;

public sealed class CorrelationMiddleware
{
    private readonly ILogger<CorrelationMiddleware> _logger;
    private readonly RequestDelegate _next;

    public CorrelationMiddleware(ILogger<CorrelationMiddleware> logger, RequestDelegate _next)
    {
        this._logger = logger;
        this._next = _next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var correlationId = GetOrCreateCorrelationId(context);

        context.Request.Headers[LoggingConstants.CorrelationIdHeader] = correlationId;
        var traceId = Activity.Current.TraceId.ToString() ?? context.TraceIdentifier;

        var logProperties = new Dictionary<string, object>
        {
            [LoggingConstants.CorrelationIdProperty] = correlationId,
            [LoggingConstants.TraceIdProperty] = traceId
        };

        using var scope = _logger.BeginScope(logProperties);
        _logger.LogDebug("Correlation context created. CorrelationId: {CorrelationId}, TraceId: {TraceId}",
            correlationId,
            traceId);

        await _next(context);
    }

    private static string GetOrCreateCorrelationId(HttpContext context)
    {
        var correlationId = context.Request.Headers[LoggingConstants.CorrelationIdHeader].FirstOrDefault();

        if (!string.IsNullOrWhiteSpace(correlationId))
        {
            return correlationId;
        }

        return Guid.NewGuid().ToString("N");

        //f47ac10b-58cc-4372-a567-0e02b2c3d479 with N the result will f47ac10b58cc4372a5670e02b2c3d479
    }
}





