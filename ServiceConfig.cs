using AuthenticationSystem.Middlewares;
using AuthenticationSystem.Repositories;
using AuthenticationSystem.Services;

namespace AuthenticationSystem;

public static class ServiceConfig
{
    public static IServiceCollection RegisterServicesAndRepositories(this IServiceCollection services)
    {
        // Services
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();

        // Repositories
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }

    public static IApplicationBuilder UseCustomMiddlewares(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ExceptionHandlingMiddleware>();
    }

    public static IServiceCollection ConfigureCors(this IServiceCollection services)
    {
        // Configure CORS
        services.AddCors(options =>
        {
            options.AddPolicy("AllowFrontend", policy =>
            {
                policy
                    .WithOrigins("http://localhost:5173") // React dev server
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials(); // if you need cookies/auth headers
            });
        });
        return services;
    }
}
