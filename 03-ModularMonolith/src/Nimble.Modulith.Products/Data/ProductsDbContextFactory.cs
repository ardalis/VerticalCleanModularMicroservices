using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Nimble.Modulith.Products.Data;

public class ProductsDbContextFactory : IDesignTimeDbContextFactory<ProductsDbContext>
{
    public ProductsDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ProductsDbContext>();
        
        // This is only used for design-time operations (migrations)
        // The actual connection string comes from Aspire at runtime
        optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=ProductsDb;Trusted_Connection=True;");
        
        return new ProductsDbContext(optionsBuilder.Options);
    }
}
