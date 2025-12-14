using System.Security.Claims;

namespace AuthenticationSystem.Security;

public static class ClaimsProperty
{
    public static Guid? GetUserId(this ClaimsPrincipal user)
    {
        var val = user.FindFirst(ClaimField.UserId)?.Value;
        return Guid.TryParse(val, out var guid) ? guid : null;
    }

    public static string? GetUsername(this ClaimsPrincipal user)
    {
        return user.FindFirst(ClaimField.Username)?.Value;
    }

    public static string? GetMobileNumber(this ClaimsPrincipal user)
    {
        return user.FindFirst(ClaimField.MobileNumber)?.Value;
    }

    public static string? GetClaim(this ClaimsPrincipal user, string claimType)
    {
        return user.FindFirst(claimType)?.Value;
    }
}