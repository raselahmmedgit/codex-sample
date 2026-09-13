using System.Security.Claims;
using ECommerce.Application.Features.Cart;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

[ApiController, Authorize]
[Route("api/cart")]
public sealed class CartController(CartService cartService) : ControllerBase
{
    [HttpGet]
    public Task<IActionResult> Get(CancellationToken cancellationToken) => Execute(x => cartService.GetCartAsync(UserId, cancellationToken));
    [HttpPost("items")]
    public Task<IActionResult> Add(AddCartItemRequest request, CancellationToken cancellationToken) => Execute(x => cartService.AddItemAsync(UserId, request, cancellationToken));
    [HttpPut("items/{id:guid}")]
    public Task<IActionResult> Update(Guid id, UpdateCartItemRequest request, CancellationToken cancellationToken) => Execute(x => cartService.UpdateItemAsync(UserId, id, request, cancellationToken));
    [HttpDelete("items/{id:guid}")]
    public Task<IActionResult> Remove(Guid id, CancellationToken cancellationToken) => Execute(x => cartService.RemoveItemAsync(UserId, id, cancellationToken));
    [HttpDelete]
    public Task<IActionResult> Clear(CancellationToken cancellationToken) => Execute(x => cartService.ClearAsync(UserId, cancellationToken));

    private Guid UserId => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    private async Task<IActionResult> Execute<T>(Func<Guid, Task<ECommerce.Application.Common.Models.Result<T>>> action) => Ok(await action(UserId));
}
