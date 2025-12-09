namespace AuthenticationSystem.Entities;

public class UserLoginToken
{
    public Guid UserLoginTokenId { get; init; } = Guid.NewGuid();

    public Guid UserId { get; init; }
    public User User { get; init; }

    public string RefreshToken { get; set; } = string.Empty;
    public DateTime RefreshTokenExpiryTime { get; set; }

    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTime? RevokedAt { get; init; }
}