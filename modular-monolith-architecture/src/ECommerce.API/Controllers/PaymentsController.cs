using ECommerce.Application.Features.Payments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

[ApiController, Authorize]
[Route("api/payments")]
public sealed class PaymentsController(PaymentService paymentService) : ControllerBase
{
    [HttpPost("initiate")]
    public async Task<IActionResult> Initiate(InitiatePaymentRequest request, CancellationToken cancellationToken) => Ok(await paymentService.InitiateAsync(request, cancellationToken));
}
