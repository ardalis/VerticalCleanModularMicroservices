using Microsoft.EntityFrameworkCore;
using OrderDemo.Core.CartAggregate;
using OrderDemo.Core.GuestUserAggregate;
using OrderDemo.Core.OrderAggregate;
using OrderDemo.Core.ProductAggregate;

namespace OrderDemo.Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
  public DbSet<Product> Products => Set<Product>();
  public DbSet<Order> Orders => Set<Order>();
  public DbSet<OrderItem> OrderItems => Set<OrderItem>();
  public DbSet<GuestUser> GuestUsers => Set<GuestUser>();
  public DbSet<Cart> Carts => Set<Cart>();
  public DbSet<CartItem> CartItems => Set<CartItem>();

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
    });

    modelBuilder.Entity<OrderItem>(b =>
    {
      b.HasKey(i => i.Id);
      b.HasOne(i => i.Order)
       .WithMany(o => o.Items)
       .HasForeignKey(i => i.OrderId);
    });

    modelBuilder.Entity<Cart>(b =>
    {
      b.HasKey(c => c.Id);
      b.HasMany(c => c.Items)
       .WithOne(i => i.Cart)
       .HasForeignKey(i => i.CartId)
       .OnDelete(DeleteBehavior.Cascade);
    });

    modelBuilder.Entity<CartItem>(b =>
    {
      b.HasKey(i => i.Id);
    });

    // Seed data
    modelBuilder.Entity<Product>().HasData(
      new Product { Id = 1, Name = "Coffee Mug", UnitPrice = 9.99m },
      new Product { Id = 2, Name = "T-Shirt", UnitPrice = 19.99m },
      new Product { Id = 3, Name = "Sticker Pack", UnitPrice = 3.99m }
    );
  }
}
