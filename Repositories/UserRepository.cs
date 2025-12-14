using AuthenticationSystem.Database;
using AuthenticationSystem.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationSystem.Repositories;

public class UserRepository(AppDbContext db) : IUserRepository
{
    public async Task<User> CreateUserAsync(User user)
    {
        await db.Users.AddAsync(user);
        await db.SaveChangesAsync();
        return user;
    }

    public Task<User?> GetUserByUserName(string username)
    {
        return Task.FromResult(db.Users.FirstOrDefault(user => user.Username.Equals(username)));
    }
    
    public async Task<User?> GetUserWithRoleByUserName(string username)
    {
        return await db.Users
            .Include(u => u.Roles)
            .FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<User?> GetUserWithLoginToken(Guid userId)
    {
        return await db.Users
            .Include(u => u.LoginToken)
            .FirstOrDefaultAsync(u => u.UserId == userId);
    }

    public async Task<List<Role>> GetRolesByRoleName(List<string> roleNames)
    {
        return await db.Roles.Where(x => roleNames.Contains(x.Name)).ToListAsync();
    }
    
    public async Task<UserLoginToken?> GetUserLoginTokenByUserIdAsync(Guid userId)
    {
        return await db.UserLoginTokens
            .FirstOrDefaultAsync(x => x.UserId == userId);
    }
    
    public async Task AddUserLoginTokenAsync(UserLoginToken token)
    {
        await db.UserLoginTokens.AddAsync(token);
    }

    public async Task SaveChangesAsync()
    {
        await db.SaveChangesAsync();
    }

    public async Task<UserLoginToken?> GetUserLoginToken(string token)
    {
        return await db.UserLoginTokens.FirstOrDefaultAsync(x => x.RefreshToken == token);
    }

    public async Task<User?> GetUserByUserId(Guid userId)
    {
        return await db.Users
            .Include(u => u.Roles)
            .FirstOrDefaultAsync(u => u.UserId == userId);
    }

    public async Task RemoveUserLoginTokenByUserId(Guid userId)
    {
        var token = await db.UserLoginTokens
            .FirstOrDefaultAsync(t => t.UserId == userId);

        if (token != null)
        {
            db.UserLoginTokens.Remove(token);
            await db.SaveChangesAsync();
        }
    }
}