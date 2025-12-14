using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationSystem.Entities;

[Index(nameof(MobileNumber), IsUnique = true)]
public class User : BaseSoftDeletableEntity
{
    public Guid UserId { get; init; } = Guid.NewGuid();
    [MaxLength(20)] public string Username { get; init; } = string.Empty;
    [MaxLength(20)] public string Email { get; init; } = string.Empty;
    [MaxLength(20)]
    [Required]
    public string MobileNumber { get; init; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public UserLoginToken LoginToken { get; init; }
    public ICollection<Role> Roles { get; set; } = new List<Role>();
}