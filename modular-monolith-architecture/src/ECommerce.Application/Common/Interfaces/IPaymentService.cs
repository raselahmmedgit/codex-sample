using ECommerce.Domain.Entities;

namespace ECommerce.Application.Common.Interfaces;

public interface IPaymentService
{
    Task<PaymentResult> InitiateAsync(Guid orderId, decimal amount, CancellationToken cancellationToken = default);
}

public sealed record PaymentResult(bool Succeeded, Guid PaymentId, string Reference, string Message);
