using ECommerce.Application.Common.Models;
using ECommerce.Application.Features.Auth.Models;

namespace ECommerce.Application.Features.Auth;

public interface IAuthService
{
    Task<Result<AuthResponse>> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default);
    Task<Result<AuthResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);
    Task<Result<AuthResponse>> RefreshAsync(RefreshTokenRequest request, CancellationToken cancellationToken = default);
    Task<Result<bool>> LogoutAsync(Guid userId, CancellationToken cancellationToken = default);
}
