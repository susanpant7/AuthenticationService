using System.ComponentModel.DataAnnotations;

namespace AuthenticationSystem.Models;

public class RegisterUserRequest
{
    [Required]
    public string Username { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty;

    public List<string> Roles { get; set; } = [RoleName.USER];
}
