using Microsoft.AspNetCore.Diagnostics;
using FluentValidation;
using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Middleware;

public sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is ValidationException validationException)
        {
            const int validationStatusCode = StatusCodes.Status400BadRequest;
            var errors = validationException.Errors?.Select(x => x.ErrorMessage).ToList() ?? [];

            var validationResponse = new ApiResponse<object>
            {
                Success = false,
                Message = "Validation failed.",
                ExceptionMessage = validationException.Message,
                Errors = errors,
                StatusCode = validationStatusCode,
                ClassName = "ApiResponse<object>"
            };

            httpContext.Response.StatusCode = validationStatusCode;
            httpContext.Response.ContentType = "application/json";

            await httpContext.Response.WriteAsJsonAsync(validationResponse, cancellationToken);
            return true;
        }

        var statusCode = exception switch
        {
            UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
            KeyNotFoundException => StatusCodes.Status404NotFound,
            ArgumentException => StatusCodes.Status400BadRequest,
            InvalidOperationException => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };

        _logger.LogError(exception, "Unhandled exception: {Path}", httpContext.Request.Path);

        var message = statusCode == StatusCodes.Status500InternalServerError
            ? "An error occurred."
            : exception.Message;

        var response = ApiResponse<object>.ErrorResult(
            message,
            exception.Message,
            statusCode,
            exception.InnerException?.Message);

        httpContext.Response.StatusCode = statusCode;
        httpContext.Response.ContentType = "application/json";

        await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);
        return true;
    }
}
