using AuthenticationSystem.Repositories;
using AuthenticationSystem.Services;

namespace AuthenticationSystem;

public static class ServiceConfig
{
    public static IServiceCollection AddServiceConfig(this IServiceCollection services)
    {
        // Services
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();

        // Repositories
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}
