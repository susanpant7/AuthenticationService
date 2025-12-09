using AuthenticationSystem.Database;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationSystem;

public static class DatabaseConfig
{
    public static IServiceCollection AddDBConnection(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(config.GetConnectionString("DefaultConnection")));
        services.AddScoped<AuditInterceptor>();
        return services;
    }
}