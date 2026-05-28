using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Nimble.Modulith.Users.Data;

public class UsersDbContext : IdentityDbContext<IdentityUser>
{
    public UsersDbContext(DbContextOptions<UsersDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        // Apply Users module specific configurations
        builder.HasDefaultSchema("Users");
        
        // Auto-discover and apply all IEntityTypeConfiguration<T> from this assembly
        builder.ApplyConfigurationsFromAssembly(typeof(UsersDbContext).Assembly);
        
        // Customize Identity tables if needed
        // For example, you can change table names:
        // builder.Entity<IdentityUser>().ToTable("Users");
        // builder.Entity<IdentityRole>().ToTable("Roles");
        // etc.
    }
}