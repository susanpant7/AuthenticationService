using AuthenticationSystem.Entities;
using AuthenticationSystem.Models;
using AuthenticationSystem.Responses;

namespace AuthenticationSystem.Services;

public interface IAuthService
{
    Task<ApiResponse<bool>> RegisterAsync(RegisterUserRequest request);
    Task<ApiResponse<TokenResponse?>> LoginAsync(LoginRequest request);
    Task<ApiResponse<TokenResponse>> RefreshTokensAsync(RefreshTokenRequest request);
}
