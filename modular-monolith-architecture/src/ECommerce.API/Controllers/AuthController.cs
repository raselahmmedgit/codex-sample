using System.Security.Claims;
using ECommerce.Application.Features.Auth;
using ECommerce.Application.Features.Auth.Models;
using ECommerce.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController(IAuthService authService, UserManager<ApplicationUser> userManager) : ControllerBase
{
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register(RegisterRequest request, CancellationToken cancellationToken) =>
        ToActionResult(await authService.RegisterAsync(request, cancellationToken));

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginRequest request, CancellationToken cancellationToken) =>
        ToActionResult(await authService.LoginAsync(request, cancellationToken));

    [HttpPost("refresh-token")]
    [AllowAnonymous]
    public async Task<IActionResult> Refresh(RefreshTokenRequest request, CancellationToken cancellationToken) =>
        ToActionResult(await authService.RefreshAsync(request, cancellationToken));

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout(CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return userId is null ? Unauthorized() : ToActionResult(await authService.LogoutAsync(Guid.Parse(userId), cancellationToken));
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> Me(CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = userId is null ? null : await userManager.FindByIdAsync(userId);
        return user is null ? Unauthorized() : Ok(new { user.Id, user.Email, user.DisplayName, Roles = await userManager.GetRolesAsync(user) });
    }

    private static IActionResult ToActionResult<T>(ECommerce.Application.Common.Models.Result<T> result) =>
        result.Succeeded ? new OkObjectResult(result) : new BadRequestObjectResult(result);
}
