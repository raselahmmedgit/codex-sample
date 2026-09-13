using ECommerce.Application.Common.Interfaces;
using ECommerce.Domain.Entities;

namespace ECommerce.Infrastructure.Payments;

public sealed class MockPaymentService(IPaymentRepository paymentRepository) : IPaymentService
{
    public async Task<PaymentResult> InitiateAsync(Guid orderId, decimal amount, CancellationToken cancellationToken = default)
    {
        if (amount <= 0) return new(false, Guid.Empty, string.Empty, "Payment amount must be greater than zero.");
        var payment = new Payment(orderId, "Mock");
        payment.ChangeStatus(ECommerce.Domain.Enums.PaymentStatus.Success);
        await paymentRepository.AddAsync(payment, cancellationToken);
        await paymentRepository.SaveChangesAsync(cancellationToken);
        return new(true, payment.Id, $"MOCK-{payment.Id:N}", "Payment completed successfully.");
    }
}
