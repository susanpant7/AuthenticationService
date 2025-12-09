using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace AuthenticationSystem;

public static class AuthConfig
{
    public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration config)
    {
        // JWT Authentication
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = config["JwtConfigs:Issuer"],

                    ValidateAudience = true,
                    ValidAudience = config["JwtConfigs:Audience"],

                    ValidateLifetime = true,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(config["JwtConfigs:AccessTokenSecret"]!))
                };
            });

        // Authorization (policy-less)
        services.AddAuthorization();

        // Global Authorization Filter (all controllers require [Authorize])
        services.AddControllers(options =>
        {
            options.Filters.Add(new AuthorizeFilter());
        });

        return services;
    }
}