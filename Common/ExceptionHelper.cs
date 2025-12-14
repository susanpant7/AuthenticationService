using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationSystem.Common;

public static class ExceptionHelper
{
    /// <summary>
    /// Maps known exceptions to appropriate HTTP status codes.
    /// Defaults to 500 Internal Server Error if unknown.
    /// </summary>
    public static int GetStatusCode(Exception? exception)
    {
        switch (exception)
        {
            case null:
                return StatusCodes.Status500InternalServerError;
            // Built-in standard exceptions
            case UnauthorizedAccessException:
                return StatusCodes.Status401Unauthorized;
            case ArgumentException:
                return StatusCodes.Status400BadRequest;
            case KeyNotFoundException:
                return StatusCodes.Status404NotFound;
            case NotImplementedException:
                return StatusCodes.Status501NotImplemented;
            case NotSupportedException:
                return StatusCodes.Status405MethodNotAllowed;
            // MVC specific exception
            case BadHttpRequestException badRequestEx:
                return badRequestEx.StatusCode;
        }

        // Optional: check for any exception with a "StatusCode" property dynamically
        var statusProp = exception.GetType().GetProperty("StatusCode");
        if (statusProp?.GetValue(exception) is int code) return code;

        // Fallback default
        return StatusCodes.Status500InternalServerError;
    }
    
    public static string GetErrorTitle(Exception exception)
    {
        return exception switch
        {
            DbUpdateException => "Database Error",
            ValidationException => "Validation Error",
            UnauthorizedAccessException => "Unauthorized",
            ArgumentException => "Invalid Argument",
            _ => "An Unhandled Error Occurred"
        };
    }
    
    public static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var statusCode = ExceptionHelper.GetStatusCode(exception);

        // Construct standard ApiResponse format
        var response = ApiResponse<string>.ErrorResponse(
            title: ExceptionHelper.GetErrorTitle(exception),
            status: (int)statusCode,
            errors: [exception.Message]
        );

        var json = JsonSerializer.Serialize(response, JsonConfig.DefaultOptions);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        return context.Response.WriteAsync(json);
    }
}