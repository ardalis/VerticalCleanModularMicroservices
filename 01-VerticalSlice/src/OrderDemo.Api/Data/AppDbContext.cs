using Microsoft.EntityFrameworkCore;
using OrderDemo.Api.Data.Entities;

namespace OrderDemo.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>(b =>
        {
            b.HasKey(o => o.Id);
            b.HasMany(o => o.Items)
             .WithOne(i => i.Order!)
             .HasForeignKey(i => i.OrderId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<OrderItem>(b =>
        {
            b.HasKey(i => i.Id);
            b.HasOne(i => i.Product)
             .WithMany()
             .HasForeignKey(i => i.ProductId);
        });

        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, Name = "Coffee Mug", UnitPrice = 9.99m },
            new Product { Id = 2, Name = "T-Shirt", UnitPrice = 19.99m },
            new Product { Id = 3, Name = "Sticker Pack", UnitPrice = 3.99m }
        );
    }
}
