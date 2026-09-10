using ECommerce.Application.Common.Logging;
using Microsoft.Extensions.Logging;

namespace ECommerce.Infrastructure.Logging;

public sealed class AppLogger<T>(ILogger<T> logger) : IAppLogger<T>
{
    public void LogInformation(string message, params object[] args) => logger.LogInformation(message, args);
    public void LogWarning(string message, params object[] args) => logger.LogWarning(message, args);
    public void LogDebug(string message, params object[] args) => logger.LogDebug(message, args);
    public void LogError(Exception exception, string message, params object[] args) => logger.LogError(exception, message, args);
}
