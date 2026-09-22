using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ECommerce.Application.Common.Models;
using ECommerce.Application.Features.Auth;
using ECommerce.Application.Features.Auth.Models;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Configuration;
using ECommerce.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ECommerce.Infrastructure.Authentication;

public sealed class AuthService(
    UserManager<ApplicationUser> userManager,
    RoleManager<ApplicationRole> roleManager,
    ApplicationDbContext dbContext,
    IOptions<JwtSettings> jwtOptions) : IAuthService
{
    private readonly JwtSettings jwtSettings = jwtOptions.Value;

    public async Task<Result<AuthResponse>> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
    {
        var user = new ApplicationUser(request.Email) { DisplayName = request.DisplayName?.Trim() };
        var result = await userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded) return Result<AuthResponse>.Failure("Registration failed.", result.Errors.Select(x => x.Description).ToArray());

        const string customerRole = "Customer";
        if (!await roleManager.RoleExistsAsync(customerRole))
        {
            var roleResult = await roleManager.CreateAsync(new ApplicationRole(customerRole));
            if (!roleResult.Succeeded && !await roleManager.RoleExistsAsync(customerRole))
            {
                await userManager.DeleteAsync(user);
                return Result<AuthResponse>.Failure("Registration failed.", roleResult.Errors.Select(x => x.Description).ToArray());
            }
        }

        var assignmentResult = await userManager.AddToRoleAsync(user, customerRole);
        if (!assignmentResult.Succeeded)
        {
            await userManager.DeleteAsync(user);
            return Result<AuthResponse>.Failure("Registration failed.", assignmentResult.Errors.Select(x => x.Description).ToArray());
        }

        return Result<AuthResponse>.Success(await IssueTokensAsync(user, cancellationToken), "Registration successful.");
    }

    public async Task<Result<AuthResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user is null) return Result<AuthResponse>.Failure("Invalid email or password.");
        if (user.LockoutEnd.HasValue && user.LockoutEnd.Value > DateTimeOffset.UtcNow)
            return Result<AuthResponse>.Failure("Account is temporarily locked.");
        if (!await userManager.CheckPasswordAsync(user, request.Password))
        {
            await userManager.AccessFailedAsync(user);
            return Result<AuthResponse>.Failure("Invalid email or password.");
        }
        await userManager.ResetAccessFailedCountAsync(user);
        return Result<AuthResponse>.Success(await IssueTokensAsync(user, cancellationToken), "Login successful.");
    }

    public async Task<Result<AuthResponse>> RefreshAsync(RefreshTokenRequest request, CancellationToken cancellationToken = default)
    {
        var hash = HashToken(request.RefreshToken);
        var storedToken = await dbContext.RefreshTokens.SingleOrDefaultAsync(x => x.TokenHash == hash, cancellationToken);
        if (storedToken is null || !storedToken.IsActive) return Result<AuthResponse>.Failure("Refresh token is invalid or expired.");
        var user = await userManager.FindByIdAsync(storedToken.UserId.ToString());
        if (user is null) return Result<AuthResponse>.Failure("User was not found.");
        storedToken.Revoke();
        var response = await IssueTokensAsync(user, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return Result<AuthResponse>.Success(response, "Token refreshed successfully.");
    }

    public async Task<Result<bool>> LogoutAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var tokens = await dbContext.RefreshTokens.Where(x => x.UserId == userId && x.RevokedAtUtc == null).ToListAsync(cancellationToken);
        foreach (var token in tokens) token.Revoke();
        await dbContext.SaveChangesAsync(cancellationToken);
        return Result<bool>.Success(true, "Logout successful.");
    }

    private async Task<AuthResponse> IssueTokensAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        var roles = await userManager.GetRolesAsync(user);
        var expiresAtUtc = DateTime.UtcNow.AddMinutes(jwtSettings.AccessTokenMinutes);
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email ?? string.Empty)
        };
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(jwtSettings.Issuer, jwtSettings.Audience, claims, expires: expiresAtUtc, signingCredentials: credentials);
        var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
        var rawRefreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        await dbContext.RefreshTokens.AddAsync(new RefreshToken(user.Id, HashToken(rawRefreshToken), DateTime.UtcNow.AddDays(jwtSettings.RefreshTokenDays)), cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return new AuthResponse(accessToken, rawRefreshToken, expiresAtUtc);
    }

    private static string HashToken(string token) => Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(token)));
}
