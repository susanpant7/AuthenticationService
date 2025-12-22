using AuthenticationSystem.Common;
using AuthenticationSystem.Models;
using AuthenticationSystem.Security;
using AuthenticationSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationSystem.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{
    //[Authorize(Roles = RoleName.SUPER_ADMIN)]
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult<bool>> Register(RegisterUserRequest request)
    {
        var response = await authService.RegisterAsync(request);
        if (!response.Success)
            return BadRequest(response);

        return Ok(response);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult> Login(LoginRequest request)
    {
        var response = await authService.LoginAsync(request);
        if (!response.Success)
            return BadRequest(response);

        //SetRefreshTokenInCookie(response);

        return Ok(response);
    }

    private void SetRefreshTokenInCookie(ApiResponse<TokenResponse> response)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTime.UtcNow.AddDays(30)
        };

        Response.Cookies.Append("refreshToken", response.Data?.RefreshToken!, cookieOptions);
        response?.Data?.RefreshToken = string.Empty;
    }

    [AllowAnonymous]
    [HttpPost("refresh")]
    public async Task<ActionResult<TokenResponse>> RefreshToken(RefreshTokenRequest refreshTokenRequest)
    {
        // Get refresh token from HttpOnly cookie
        // if (!Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
        // {
        //     return Unauthorized(new { success = false, message = "No refresh token" });
        // }
        if (string.IsNullOrWhiteSpace(refreshTokenRequest.RefreshToken))
        {
            return Unauthorized(new { success = false, message = "No refresh token" });
        }

        var response = await authService.RefreshTokensAsync(refreshTokenRequest.RefreshToken);
        if (!response.Success)
            return BadRequest(response);

        //SetRefreshTokenInCookie(response);
        return Ok(response);
    }
    
    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var userId = User.GetUserId();
        if (userId == null)
            return Unauthorized();
        await authService.LogoutUser(userId??Guid.Empty);
        // Get refresh token from cookie
        if (Request.Cookies.ContainsKey("refreshToken"))
        {
            Response.Cookies.Append("refreshToken", "", new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(-1), // expire immediately
                Secure = true,
                SameSite = SameSiteMode.None
            });
        }

        return Ok(new { success = true });
    }

}