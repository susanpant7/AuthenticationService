using AuthenticationSystem.Entities;

namespace AuthenticationSystem.Services;

public interface IUserService
{
    Task<User?> GetUserByUserName(string userName);
    Task<User?> GetUserWithRolesByUserName(string userName);
    Task<User> CreateUserAsync(User user, List<Role> roles);
    Task<User?> GetUserWithLoginToken(Guid userId);
    Task SaveOrUpdateUserRefreshToken(Guid userId, string refreshToken);
    Task<List<Role>> GetRolesByRoleName(List<string> roleNames);
}