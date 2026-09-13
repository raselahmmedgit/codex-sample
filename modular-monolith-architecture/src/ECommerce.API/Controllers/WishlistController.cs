using System.Security.Claims;
using ECommerce.Application.Features.Cart;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

[ApiController, Authorize]
[Route("api/wishlist")]
public sealed class WishlistController(CartService cartService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken) => Ok(await cartService.GetWishlistAsync(UserId, cancellationToken));
    [HttpPost("items/{productId:guid}")]
    public async Task<IActionResult> Add(Guid productId, CancellationToken cancellationToken) => Ok(await cartService.AddWishlistItemAsync(UserId, productId, cancellationToken));
    [HttpDelete("items/{id:guid}")]
    public async Task<IActionResult> Remove(Guid id, CancellationToken cancellationToken) => Ok(await cartService.RemoveWishlistItemAsync(UserId, id, cancellationToken));
    private Guid UserId => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
}
