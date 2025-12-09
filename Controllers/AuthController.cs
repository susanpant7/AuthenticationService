using AuthenticationSystem.Entities;
using AuthenticationSystem.Models;
using AuthenticationSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationSystem.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{
    [Authorize(Roles = RoleName.SUPER_ADMIN)]
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
    public async Task<ActionResult<TokenResponse>> Login(LoginRequest request)
    {
        var response = await authService.LoginAsync(request);
        if (!response.Success)
            return BadRequest(response);

        return Ok(response);
    }
    
    [AllowAnonymous]
    [HttpPost("refresh-token")]
    public async Task<ActionResult<TokenResponse>> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        var response = await authService.RefreshTokensAsync(request);
        if (!response.Success)
            return BadRequest(response);

        return Ok(response);
    }
}