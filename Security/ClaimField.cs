using System.Security.Claims;

namespace AuthenticationSystem.Security;

public static class ClaimField
{
    // Standard claims
    public const string UserId = ClaimTypes.NameIdentifier;
    public const string Username = ClaimTypes.Name;
    public const string Role = ClaimTypes.Role;

    // Custom claims (add as needed)
    public const string MobileNumber = "mobileNumber";
    public const string Email = ClaimTypes.Email;
}