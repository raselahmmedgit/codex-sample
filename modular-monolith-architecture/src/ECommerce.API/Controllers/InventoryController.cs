using ECommerce.Application.Features.Inventory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

[ApiController, Authorize(Roles = "Admin,Manager,Seller")]
[Route("api/inventory")]
public sealed class InventoryController(InventoryService inventoryService) : ControllerBase
{
    [HttpPost("adjust")]
    public async Task<IActionResult> Adjust(AdjustInventoryRequest request, CancellationToken cancellationToken) => Ok(await inventoryService.AdjustAsync(request, cancellationToken));
    [HttpPost("reserve")]
    public async Task<IActionResult> Reserve(ReserveInventoryRequest request, CancellationToken cancellationToken) => Ok(await inventoryService.ReserveAsync(request, cancellationToken));
    [HttpPost("release")]
    public async Task<IActionResult> Release(ReserveInventoryRequest request, CancellationToken cancellationToken) => Ok(await inventoryService.ReleaseAsync(request, cancellationToken));
}
