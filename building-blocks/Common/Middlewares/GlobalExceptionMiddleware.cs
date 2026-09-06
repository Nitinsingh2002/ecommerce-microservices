using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace buildingblocks.Common.Middlewares;

public sealed class GlobalExceptionMiddleware
{
    private readonly ILogger<GlobalExceptionMiddleware> _logger;
    private readonly RequestDelegate _next;

    public GlobalExceptionMiddleware(ILogger<GlobalExceptionMiddleware> _logger, RequestDelegate _next)
    {
        this._logger = _logger;
        this._next = _next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
           await _next(context);
        }catch(Exception ex)
        {
            _logger.LogError(
                ex,
                "Unhandled exception occurred while processing request {Method} {Path}",
                context.Request.Method,
                context.Request.Path
                );

            await HandleExceptionAsync(context);
        }
    }

    public static async Task HandleExceptionAsync (HttpContext context)
    {
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        context.Response.ContentType =
     "application/json";

        var response = new
        {
            error = new
            {
                Code = "INTERNAL_SERVER_ERROR",
                Message ="An Unexpected error occured"
            }
        };


        await context.Response.WriteAsync(
            JsonSerializer.Serialize(response));
    }

}

