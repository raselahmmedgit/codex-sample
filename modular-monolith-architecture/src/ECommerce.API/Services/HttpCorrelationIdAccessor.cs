using ECommerce.Application.Common.Interfaces;

namespace ECommerce.API.Services;

public sealed class HttpCorrelationIdAccessor(IHttpContextAccessor httpContextAccessor) : ICorrelationIdAccessor
{
    public string CorrelationId => httpContextAccessor.HttpContext?.TraceIdentifier ?? "no-request";
}
