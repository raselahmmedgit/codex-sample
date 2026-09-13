using System.Net;
using System.Text.Json;
using ECommerce.Application.Common.Exceptions;

namespace ECommerce.API.Middleware;

public sealed class ExceptionHandlingMiddleware(
    RequestDelegate next,
    ILogger<ExceptionHandlingMiddleware> logger,
    IHostEnvironment environment)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try { await next(context); }
        catch (Exception exception)
        {
            logger.LogError(exception, "Unhandled exception. CorrelationId: {CorrelationId}", context.TraceIdentifier);
            await WriteErrorAsync(context, exception, environment.IsDevelopment());
        }
    }

    private static async Task WriteErrorAsync(HttpContext context, Exception exception, bool isDevelopment)
    {
        var statusCode = exception switch
        {
            ValidationException => (int)HttpStatusCode.UnprocessableEntity,
            NotFoundException => (int)HttpStatusCode.NotFound,
            ConflictException => (int)HttpStatusCode.Conflict,
            UnauthorizedException => (int)HttpStatusCode.Unauthorized,
            ForbiddenException => (int)HttpStatusCode.Forbidden,
            ArgumentException => (int)HttpStatusCode.BadRequest,
            _ => (int)HttpStatusCode.InternalServerError
        };
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";
        var message = statusCode == 500 ? "An unexpected error occurred." : exception.Message;
        var errors = isDevelopment && statusCode == 500
            ? new[] { exception.Message }
            : Array.Empty<string>();
        await context.Response.WriteAsync(JsonSerializer.Serialize(new
        {
            success = false, message, errors, correlationId = context.TraceIdentifier
        }));
    }
}
