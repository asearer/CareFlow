using System.Net;
using System.Text.Json;
using CareFlow.Domain.Exceptions;

namespace CareFlow.Api.Middleware;

public class ApiExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ApiExceptionMiddleware> _logger;

    public ApiExceptionMiddleware(RequestDelegate next, ILogger<ApiExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception has occurred.");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var statusCode = StatusCodes.Status500InternalServerError;
        var title = "An error occurred while processing your request.";
        IDictionary<string, object?>? extensions = null;

        switch (exception)
        {
            case ValidationException validationEx:
                statusCode = StatusCodes.Status400BadRequest;
                title = "Validation Failed";
                extensions = new Dictionary<string, object?>
                {
                    { "errors", validationEx.Errors }
                };
                break;
            case DomainException domainEx:
                statusCode = StatusCodes.Status400BadRequest;
                title = domainEx.Message;
                break;
            case UnauthorizedAccessException:
                statusCode = StatusCodes.Status401Unauthorized;
                title = "Unauthorized";
                break;
            case KeyNotFoundException:
                statusCode = StatusCodes.Status404NotFound;
                title = "Resource Not Found";
                break;
        }

        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = statusCode;

        var problemDetails = new Microsoft.AspNetCore.Mvc.ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = exception.Message,
            Instance = context.Request.Path
        };

        if (extensions != null)
        {
            foreach (var ext in extensions)
            {
                problemDetails.Extensions.Add(ext.Key, ext.Value);
            }
        }

        await context.Response.WriteAsJsonAsync(problemDetails);
    }
}
