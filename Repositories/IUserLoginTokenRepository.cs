using AuthenticationSystem.Entities;

namespace AuthenticationSystem.Repositories;

public interface IUserLoginTokenRepository
{
    Task<UserLoginToken?> GetByUserIdAsync(Guid userId);
    Task AddAsync(UserLoginToken token);
    Task SaveChangesAsync();
}