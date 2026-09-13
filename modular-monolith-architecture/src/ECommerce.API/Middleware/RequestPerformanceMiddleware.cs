using System.Diagnostics;
using ECommerce.Application.Common.Logging;
using ECommerce.Infrastructure.Configuration;
using Microsoft.Extensions.Options;

namespace ECommerce.API.Middleware;

public sealed class RequestPerformanceMiddleware(
    RequestDelegate next,
    IAppLogger<RequestPerformanceMiddleware> logger,
    IOptions<SerilogSettings> options)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        await next(context);
        stopwatch.Stop();

        if (stopwatch.ElapsedMilliseconds < options.Value.SlowRequestThresholdMilliseconds) return;
        logger.LogWarning(
            "Slow request detected. Method: {Method}, Path: {Path}, StatusCode: {StatusCode}, DurationMs: {DurationMs}, CorrelationId: {CorrelationId}",
            context.Request.Method,
            context.Request.Path,
            context.Response.StatusCode,
            stopwatch.ElapsedMilliseconds,
            context.TraceIdentifier);
    }
}
