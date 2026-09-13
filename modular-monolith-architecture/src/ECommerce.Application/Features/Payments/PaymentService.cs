using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.Common.Models;

namespace ECommerce.Application.Features.Payments;

public sealed record InitiatePaymentRequest(Guid OrderId, decimal Amount);
public sealed record PaymentResponse(Guid PaymentId, string Reference, string Message);

public sealed class PaymentService(IPaymentService paymentService)
{
    public async Task<Result<PaymentResponse>> InitiateAsync(InitiatePaymentRequest request, CancellationToken cancellationToken)
    {
        var result = await paymentService.InitiateAsync(request.OrderId, request.Amount, cancellationToken);
        return result.Succeeded
            ? Result<PaymentResponse>.Success(new(result.PaymentId, result.Reference, result.Message), result.Message)
            : Result<PaymentResponse>.Failure(result.Message);
    }
}
