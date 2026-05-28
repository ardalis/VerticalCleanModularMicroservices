using Microsoft.EntityFrameworkCore;
using Nimble.Modulith.Products.Models;

namespace Nimble.Modulith.Products.Data;

public class ProductsDbContext : DbContext
{
    public ProductsDbContext(DbContextOptions<ProductsDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        // Apply Products module specific configurations
        builder.HasDefaultSchema("Products");
        
        // Apply all configurations from this assembly
        builder.ApplyConfigurationsFromAssembly(typeof(ProductsDbContext).Assembly);
    }
}
