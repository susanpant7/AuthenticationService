using AuthenticationSystem.Database;
using AuthenticationSystem.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationSystem.Repositories;

public class UserLoginTokenRepository(AppDbContext db) : IUserLoginTokenRepository
{
    public Task<UserLoginToken?> GetByUserIdAsync(Guid userId)
    {
        return db.UserLoginTokens
            .FirstOrDefaultAsync(x => x.UserId == userId);
    }

    public Task AddAsync(UserLoginToken token)
    {
        return db.UserLoginTokens.AddAsync(token).AsTask();
    }

    public Task SaveChangesAsync()
    {
        return db.SaveChangesAsync();
    }
}
