using System.Security.Claims;

namespace AuthenticationSystem.Models;

public class TokenResponse
{
    public required string AccessToken { get; set; }
    public required string RefreshToken { get; set; }
    public int AccessTokenExpiryInMinute { get; set; }
    public int RefreshTokenExpiryInDays { get; set; }
}

