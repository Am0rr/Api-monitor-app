using System.Net;
using System.Text.Json;
using AM.BLL.Exceptions;
using FluentValidation;

namespace AM.API.Middleware;

public class GlobalExceptionHandler(
    RequestDelegate requestDelegate,
    ILogger<GlobalExceptionHandler> logger)
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await requestDelegate(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unhandled exception occurred");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var statusCode = exception switch
        {
            ArgumentException => (int)HttpStatusCode.BadRequest,
            InvalidOperationException => (int)HttpStatusCode.BadRequest,
            KeyNotFoundException => (int)HttpStatusCode.NotFound,
            UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
            ValidationException => (int)HttpStatusCode.BadRequest,
            NotFoundException => (int)HttpStatusCode.NotFound,
            _ => (int)HttpStatusCode.InternalServerError
        };

        object response;

        if (exception is ValidationException validationException)
        {
            response = new
            {
                status = statusCode,
                error = "Bad Request",
                errors = validationException.Errors.Select(e => e.ErrorMessage) // 💡 Исправлена опечатка (errors)
            };
        }
        else
        {
            response = new
            {
                status = statusCode,
                error = GetTitle(statusCode),
                message = exception.Message
            };
        }

        context.Response.StatusCode = statusCode;
        return context.Response.WriteAsync(JsonSerializer.Serialize(response, JsonOptions));
    }
    
    private static string GetTitle(int statusCode) => statusCode switch
    {
        400 => "Bad Request",
        401 => "Unauthorized",
        403 => "Forbidden",
        404 => "Not Found",
        409 => "Conflict",
        500 => "Internal Server Error",
        _ => "An Error Occurred"
    };
}