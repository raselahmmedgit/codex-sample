using ECommerce.Application.Features.Coupons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

[ApiController, Authorize]
[Route("api/coupons")]
public sealed class CouponsController(CouponService couponService) : ControllerBase
{
    [HttpPost("validate")]
    public async Task<IActionResult> Validate(ValidateCouponRequest request, CancellationToken cancellationToken) => Ok(await couponService.ValidateAsync(request, cancellationToken));

    [HttpPost]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Create(CreateCouponRequest request, CancellationToken cancellationToken) => Ok(await couponService.CreateAsync(request, cancellationToken));
}
