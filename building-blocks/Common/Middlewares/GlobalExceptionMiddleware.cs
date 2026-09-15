
using System.Net;
using System.Text.Json;
using Common.Responses;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using static System.Net.WebRequestMethods;
using static System.Text.Json.JsonSerializer;

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
        }
        // To handle valiation exception thrown by fluent validation, ValidationException  this class belongs to fluent validation
        catch (ValidationException ex)
        {
            await HandleValidationExceptionAsync(context, ex);
        }
        //To handle Unhandled exception thrown by the application
        catch (Exception ex)
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

    public static async Task HandleExceptionAsync(HttpContext context)
    {
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        context.Response.ContentType =
     "application/json";

        // var response = new
        // {
        //     error = new
        //     {
        //         Code = "INTERNAL_SERVER_ERROR",
        //         Message = "An Unexpected error occured"
        //     }
        // };

        // using centralized error response class to return error response in a structured format instead of using anonymous objcet as shown above
        var response = new ApiErrorResponse(
            new ApiError("INTERNAL_SERVER_ERROR", "An Unexpected error occured")
        );


        var jsonResponse = JsonSerializer.Serialize(response);
        await context.Response.WriteAsync(jsonResponse);
    }

    public static async Task HandleValidationExceptionAsync(HttpContext context, ValidationException ex)
    {
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        context.Response.ContentType = "application/json";
        var response = new
        {
            error = new
            {
                code = "VALIDATION_ERROR",
                message = "One or more validation errors occurred.",
                details = ex.Errors.Select(e => new
                {
                    field = e.PropertyName,
                    message = e.ErrorMessage
                }).ToList()
            }
        };

        var jsonResponse = JsonSerializer.Serialize(response);
        await context.Response.WriteAsync(jsonResponse);
    }
}

