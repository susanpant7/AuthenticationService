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
            .AsNoTracking()
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
}