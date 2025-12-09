using System.Linq.Expressions;
using System.Security.Claims;
using AuthenticationSystem.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationSystem.Database;

public class AppDbContext(DbContextOptions<AppDbContext> options,
    AuditInterceptor auditInterceptor) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserLoginToken> UserLoginTokens { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(auditInterceptor);
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureManyToManyEntities(modelBuilder);
        
        base.OnModelCreating(modelBuilder);

        SeedData(modelBuilder);
    }

    private static void ConfigureManyToManyEntities(ModelBuilder modelBuilder)
    {
        // Configure many-to-many relationship
        // Just to rename the auto created table from RoleUser to UserRole
        modelBuilder.Entity<User>()
            .HasMany(u => u.Roles)
            .WithMany(r => r.Users)
            .UsingEntity(j => j.ToTable("UserRoles"));
    }

    private static void SeedData(ModelBuilder modelBuilder)
    {
        // Seed roles
        modelBuilder.Entity<Role>().HasData(
            new Role { RoleId = 1, Name = "ADMIN" },
            new Role { RoleId = 2, Name = "USER" },
            new Role { RoleId = 3, Name = "SUPER_ADMIN" }
        );
    }
}