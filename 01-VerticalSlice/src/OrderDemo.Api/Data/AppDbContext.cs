using Microsoft.EntityFrameworkCore;
using OrderDemo.Api.Data.Entities;

namespace OrderDemo.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<GuestUser> GuestUsers => Set<GuestUser>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GuestUser>(b =>
        {
            b.HasKey(g => g.Id);
            b.Property(g => g.Email).IsRequired();
        });

        modelBuilder.Entity<Order>(b =>
        {
            b.HasKey(o => o.Id);
            b.HasOne(o => o.GuestUser)
             .WithMany()
             .HasForeignKey(o => o.GuestUserId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<OrderItem>(b =>
        {
            b.HasKey(i => i.Id);
            b.HasOne(i => i.Product)
             .WithMany()
             .HasForeignKey(i => i.ProductId);
        });

        // Seed data
        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, Name = "Coffee Mug", UnitPrice = 9.99m },
            new Product { Id = 2, Name = "T-Shirt", UnitPrice = 19.99m },
            new Product { Id = 3, Name = "Sticker Pack", UnitPrice = 3.99m }
        );
    }
}
