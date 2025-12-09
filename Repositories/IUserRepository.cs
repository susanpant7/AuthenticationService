using AuthenticationSystem.Entities;

namespace AuthenticationSystem.Repositories;

public interface IUserRepository
{
    Task<User> CreateUserAsync(User user);
    Task<User?> GetUserByUserName(string username);
    Task<User?> GetUserWithRoleByUserName(string username);
    Task<List<Role>> GetRolesByRoleName(List<string> roleNames);
    Task<User?> GetUserWithLoginToken(Guid userId);
    Task<UserLoginToken?> GetUserLoginTokenByUserIdAsync(Guid userId);
    Task AddUserLoginTokenAsync(UserLoginToken token);
    Task SaveChangesAsync();
}