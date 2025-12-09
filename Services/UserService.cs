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
    
    // TODO: Move to roles service class
    public async Task<List<Role>> GetRolesByRoleName(List<string> roleNames)
    {
        return await userRepository.GetRolesByRoleName(roleNames);
    }

    // TODO: Move to user login token service class
    public async Task SaveOrUpdateUserRefreshToken(Guid userId, string refreshToken)
    {
        // fetch login token record
        var loginToken = await userRepository.GetUserLoginTokenByUserIdAsync(userId);

        if (loginToken == null)
        {
            loginToken = new UserLoginToken
            {
                UserId = userId,
                RefreshToken = refreshToken,
                RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7) //TODO: dont use hardcoded number
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