

using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Common.Middlewares;

public sealed class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate _next, ILogger<RequestLoggingMiddleware> _looger)
    {
        this._next = _next;
        this._logger = _looger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        var request = context.Request;

        _logger.LogInformation(
            "HTTP request started. Method: {Method}, Path: {Path}",
            request.Method,
            request.Path);

        await _next(context);

        stopwatch.Stop();

        var statusCode = context.Response.StatusCode;

        if (statusCode >= 500)
        {
            _logger.LogError(
                "HTTP request completed with server error. Method: {HttpMethod}, Path: {RequestPath}, StatusCode: {StatusCode}, DurationMs: {DurationMs}",
                request.Method,
                request.Path,
                statusCode,
                stopwatch.ElapsedMilliseconds);
        }
        else if (statusCode >= 400)
        {
            _logger.LogWarning(
                "HTTP request completed with client error. Method: {HttpMethod}, Path: {RequestPath}, StatusCode: {StatusCode}, DurationMs: {DurationMs}",
                request.Method,
                request.Path,
                statusCode,
                stopwatch.ElapsedMilliseconds);
        }
        else
        {
            _logger.LogInformation(
                "HTTP request completed successfully. Method: {HttpMethod}, Path: {RequestPath}, StatusCode: {StatusCode}, DurationMs: {DurationMs}",
                request.Method,
                request.Path,
                statusCode,
                stopwatch.ElapsedMilliseconds);
        }
    }
}