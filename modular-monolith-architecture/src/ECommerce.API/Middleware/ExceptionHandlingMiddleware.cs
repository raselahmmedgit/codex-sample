using System.Net;
using System.Text.Json;
using ECommerce.Application.Common.Exceptions;

namespace ECommerce.API.Middleware;

public sealed class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try { await next(context); }
        catch (Exception exception)
        {
            logger.LogError(exception, "Unhandled exception. CorrelationId: {CorrelationId}", context.TraceIdentifier);
            await WriteErrorAsync(context, exception);
        }
    }

    private static async Task WriteErrorAsync(HttpContext context, Exception exception)
    {
        var statusCode = exception switch
        {
            ValidationException => (int)HttpStatusCode.UnprocessableEntity,
            NotFoundException => (int)HttpStatusCode.NotFound,
            ConflictException => (int)HttpStatusCode.Conflict,
            _ => (int)HttpStatusCode.InternalServerError
        };
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";
        var message = statusCode == 500 ? "An unexpected error occurred." : exception.Message;
        await context.Response.WriteAsync(JsonSerializer.Serialize(new
        {
            success = false, message, errors = Array.Empty<string>(), correlationId = context.TraceIdentifier
        }));
    }
}
