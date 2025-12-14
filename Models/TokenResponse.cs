using System.Security.Claims;

namespace AuthenticationSystem.Models;

public class TokenResponse
{
    public required string AccessToken { get; set; }
    public required string RefreshToken { get; set; }
}

