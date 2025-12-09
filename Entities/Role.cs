namespace AuthenticationSystem.Entities;

public class Role
{
    public int RoleId { get; set; }
    public string Name { get; set; } = string.Empty;

    // Navigation property (many-to-many)
    public ICollection<User> Users { get; set; } = new List<User>();
}