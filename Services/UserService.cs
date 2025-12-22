using AuthenticationSystem.Entities;
using AuthenticationSystem.Repositories;

namespace AuthenticationSystem.Services;

public class UserService(IUserRepository userRepository) : IUserService
{
    public async Task<User?> GetUserByUserName(string userName)
    {
        return await userRepository.GetUserByUserName(userName);
    }
    
    public async Task<User?> GetUserWithRolesByUserName(string userName)
    {
        return await userRepository.GetUserWithRoleByUserName(userName);
    }

    public async Task<User> CreateUserAsync(User user, List<Role> roles)
    {
        user.Roles = roles;
        return await userRepository.CreateUserAsync(user);
    }

    public async Task<User?> GetUserWithLoginToken(Guid userId)
    {
        return await userRepository.GetUserWithLoginToken(userId);
    }
    
    public async Task<UserLoginToken?> GetUserLoginTokenByToken(string token)
    {
        return await userRepository.GetUserLoginToken(token);
    }

    public async Task<User?> GetUserByUserId(Guid userId)
    {
        return await userRepository.GetUserByUserId(userId);
    }

    public async Task RemoveUserLoginTokenByUserId(Guid userId)
    {
        await userRepository.RemoveUserLoginTokenByUserId(userId);
    }

    // TODO: Move to roles service class
    public async Task<List<Role>> GetRolesByRoleName(List<string> roleNames)
    {
        return await userRepository.GetRolesByRoleName(roleNames);
    }

    // TODO: Move to user login token service class
    public async Task SaveOrUpdateUserRefreshToken(Guid userId, string refreshToken, int expiryInDays)
    {
        // fetch login token record
        var loginToken = await userRepository.GetUserLoginTokenByUserIdAsync(userId);

        if (loginToken == null)
        {
            loginToken = new UserLoginToken
            {
                UserId = userId,
                RefreshToken = refreshToken,
                RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(expiryInDays)
            };

            await userRepository.AddUserLoginTokenAsync(loginToken);
        }
        else
        {
            loginToken.RefreshToken = refreshToken;
            loginToken.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        }

        await userRepository.SaveChangesAsync();
    }
}