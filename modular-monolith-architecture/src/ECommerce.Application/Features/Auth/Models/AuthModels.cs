using System.ComponentModel.DataAnnotations;

namespace ECommerce.Application.Features.Auth.Models;

public sealed record RegisterRequest(
    [property: Required, EmailAddress, StringLength(256)] string Email,
    [property: Required, StringLength(128, MinimumLength = 8)] string Password,
    [property: StringLength(100)] string? DisplayName);

public sealed record LoginRequest(
    [property: Required, EmailAddress, StringLength(256)] string Email,
    [property: Required, StringLength(128, MinimumLength = 8)] string Password);

public sealed record RefreshTokenRequest([property: Required] string RefreshToken);
public sealed record AuthResponse(string AccessToken, string RefreshToken, DateTime AccessTokenExpiresAtUtc);
