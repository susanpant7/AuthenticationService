using AuthenticationSystem.Common;
using AuthenticationSystem.Entities;
using AuthenticationSystem.Models;

namespace AuthenticationSystem.Services;

public interface IAuthService
{
    Task<ApiResponse<bool>> RegisterAsync(RegisterUserRequest request);
    Task<ApiResponse<TokenResponse>> LoginAsync(LoginRequest request);
    Task<ApiResponse<TokenResponse>> RefreshTokensAsync(string refreshToken);
    Task LogoutUser(Guid userId);
}
