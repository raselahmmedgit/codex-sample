using System.Security.Claims;
using ECommerce.Application.Features.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

[ApiController, Authorize]
[Route("api/orders")]
public sealed class OrdersController(OrderService orderService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(CreateOrderRequest request, CancellationToken cancellationToken) => Ok(await orderService.CreateAsync(UserId, request, cancellationToken));
    [HttpGet]
    public async Task<IActionResult> List(CancellationToken cancellationToken) => Ok(await orderService.ListAsync(UserId, cancellationToken));
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken) => Ok(await orderService.GetAsync(UserId, id, IsManager, cancellationToken));
    [HttpPatch("{id:guid}/status")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> ChangeStatus(Guid id, ChangeOrderStatusRequest request, CancellationToken cancellationToken) => Ok(await orderService.ChangeStatusAsync(id, request, cancellationToken));
    private Guid UserId => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    private bool IsManager => User.IsInRole("Admin") || User.IsInRole("Manager");
}
