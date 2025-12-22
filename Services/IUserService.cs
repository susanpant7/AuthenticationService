using AuthenticationSystem.Entities;

namespace AuthenticationSystem.Services;

public interface IUserService
{
    Task<User?> GetUserByUserName(string userName);
    Task<User?> GetUserWithRolesByUserName(string userName);
    Task<User> CreateUserAsync(User user, List<Role> roles);
    Task<User?> GetUserWithLoginToken(Guid userId);
    Task SaveOrUpdateUserRefreshToken(Guid userId, string refreshToken, int expiresIn);
    Task<List<Role>> GetRolesByRoleName(List<string> roleNames);
    Task<UserLoginToken?> GetUserLoginTokenByToken(string token);
    Task<User?> GetUserByUserId(Guid userId);
    Task RemoveUserLoginTokenByUserId(Guid userId);
}